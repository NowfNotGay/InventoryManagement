using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Method
{
    public class General
    {
        public static string encrytion_pass(string input)
        {
            string str_SHA1 = "";
            byte[] encryption_resuft = System.Text.Encoding.UTF8.GetBytes(input);
            SHA1CryptoServiceProvider encrytion_provider = new SHA1CryptoServiceProvider();
            encryption_resuft = encrytion_provider.ComputeHash(encryption_resuft);

            foreach (byte resuft in encryption_resuft)
            {
                str_SHA1 += resuft.ToString();
            }
            return str_SHA1;
        }
        public static string encryption_refreshToken()
        {
            //string str_SHA1 = "";
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            //string str = new string(Enumerable.Range(1, 50)
            //    .Select(_ => chars[random.Next(chars.Length)]).ToArray());
            //byte[] encryption_result = System.Text.Encoding.UTF8.GetBytes(str);
            //SHA1CryptoServiceProvider encrytion_provider = new SHA1CryptoServiceProvider();
            //encryption_result = encrytion_provider.ComputeHash(encryption_result);

            //foreach (byte result in encryption_result)
            //{
            //    str_SHA1 += result.ToString();
            //}
            //return str_SHA1;
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[64];
                rng.GetBytes(tokenData);

                string token = Convert.ToBase64String(tokenData);
                return token;
            }
        }
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static DataTable ConvertToDataTable1<T>(T data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            DataRow row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
                row[prop.Name] = prop.GetValue(data) ?? DBNull.Value;
            table.Rows.Add(row);

            return table;
        }

        public static DataTable ToDataTable<T>(List<T> iList)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);


                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach (T iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static List<T> ConvertToListEnableNull<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                    {
                        PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                        if (row[pro.Name] != DBNull.Value)
                        {
                            pro.SetValue(objT, row[pro.Name] = Convert.ChangeType(row[pro.Name], Nullable.GetUnderlyingType(pI.PropertyType) ?? pI.PropertyType));
                        }
                        else
                        {
                            pro.SetValue(objT, null);

                        }
                        //pro.SetValue(objT, row[pro.Name] = Convert.ChangeType(row[pro.Name], Nullable.GetUnderlyingType(pI.PropertyType) ?? pI.PropertyType));
                    }
                }
                return objT;
            }).ToList();
        }

        #region Encryption Decryption Using AES algorithm

        public static string EncryptString(string textToEncrypt)
        {
            try
            {
                string ToReturn = "";
                string publickey = "kanako99";
                string secretkey = "ozawa199";
                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey.Substring(0, 8));
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey.Substring(0, 8));
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public static string DecryptString(string textToDecrypt)
        {
            try
            {
                string ToReturn = "";
                string publickey = "kanako99";
                string secretkey = "ozawa199";
                byte[] privatekeyByte = { };
                privatekeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }
        }

        #endregion Encryption Decryption Using AES algorithm
    }

    public enum ResponseCode
    {
        Success = 0,
        NotFound = 1,
        InvalidInput = 2,
        Unauthorized = 3,
        InternalError = 4,
        FailToConnectDB = 5,
        FailWhileExecutingStoredProcedure = 6,
        UnhandledError=999
    }
}
