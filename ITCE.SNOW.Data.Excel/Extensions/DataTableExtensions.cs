using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using ITCE.SNOW.Domain;

namespace ITCE.SNOW.Data.Excel.Extensions { 
    public static class DataTableExtensions
    {
        /// <summary>
        /// Converts datatable to list<T/> dynamically
        /// </summary>
        /// <typeparam name="T">Class name</typeparam>
        /// <param name="dataTable">data table to convert</param>
        /// <returns>List<T/></returns>
        public static List<T> ToList<T>(this DataTable dataTable) where T : DomainModelBase, new()
        {
            var dataList = new List<T>();
                
            //Define what attributes to be read from the class
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

            //Read Attribute Names and Types
            var objFieldNames = typeof(T).GetProperties(flags).
                Select(item => new
                {
                    Name = item.Name,
                    ColumnName = item.GetCustomAttributes<DataMemberAttribute>()?.FirstOrDefault()?.Name,
                    Type = Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType
                }).ToList();

            //Read Datatable column names and types
            var fieldNames = dataTable.Columns.Cast<DataColumn>().
                Select(item => new {
                    Name = item.ColumnName,
                    Type = item.DataType
                }).ToList();

            foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
            {
                var classObj = new T();
                classObj.Reference = dataRow.ToReference();
                foreach (var dtField in fieldNames)
                {
                    //Try and get the field by column name or just property name if no datamember defined
                    var fieldName = objFieldNames.FirstOrDefault(n => n.ColumnName == dtField.Name)?.Name ??
                                    dtField.Name;
                    PropertyInfo propertyInfos = classObj.GetType().GetProperty(fieldName);
                    //If it is not there, get its name from the DataMember property
                    
                    var field = objFieldNames.Find(x => x.Name == dtField.Name || x.ColumnName == dtField.Name);

                    if (field != null)
                    {

                        if (propertyInfos.PropertyType == typeof(DateTime))
                        {
                            propertyInfos.SetValue
                                (classObj, ConvertToDateTime(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(int))
                        {
                            propertyInfos.SetValue
                                (classObj, ConvertToInt(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(long))
                        {
                            propertyInfos.SetValue
                                (classObj, ConvertToLong(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(decimal))
                        {
                            propertyInfos.SetValue
                                (classObj, ConvertToDecimal(dataRow[dtField.Name]), null);
                        }
                        else if (propertyInfos.PropertyType == typeof(User))
                        {
                            propertyInfos.SetValue
                                (classObj, new User(ConvertToString(dataRow[dtField.Name].ToString().Trim())));
                        }
                        else if (propertyInfos.PropertyType == typeof(string))
                        {
                            if (dataRow[dtField.Name] is DateTime)
                            {
                                propertyInfos.SetValue
                                    (classObj, ConvertToDateString(dataRow[dtField.Name]), null);
                            }
                            else
                            {
                                propertyInfos.SetValue
                                    (classObj, ConvertToString(dataRow[dtField.Name]), null);
                            }
                        }
                    }
                }
                dataList.Add(classObj);
            }
            return dataList;
        }

        private static string ConvertToDateString(object date)
        {
            if (date == null)
                return string.Empty;

            return Convert.ToDateTime(date).ToString(CultureInfo.InvariantCulture);
        }

        private static string ConvertToString(object value)
        {
            return value.ToString().Trim();
        }

        private static int ConvertToInt(object value)
        {
            return Convert.ToInt32(value ?? 0);
        }

        private static long ConvertToLong(object value)
        {
            return Convert.ToInt64(value ?? 0);
        }

        private static decimal ConvertToDecimal(object value)
        {
            return Convert.ToDecimal(value ?? 0);
        }

        private static DateTime ConvertToDateTime(object date)
        {
            return Convert.ToDateTime(date ?? DateTime.MinValue);
        }
    }
}
