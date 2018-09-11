using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForeignExchance.Data;

namespace ForeignExchance.Services
{
    public class DataService
    {
        public bool DeleteAll<T>() where T:class
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var oldRecords = da.GetList<T>(false);
                    foreach (var oldRecord in oldRecords)
                    {
                        da.Delete<T>(oldRecord);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }

        public T DeleteAllAndInsert<T>(T model) where T : class
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var oldRecords = da.GetList<T>(false);
                    foreach (var oldRecord in oldRecords)
                    {
                        da.Delete<T>(oldRecord);
                    }

                    da.Insert(model);
                    return model;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return model;
            }
        }

        public T InsertOrUpdate<T>(T model) where T : class
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var oldRecord = da.Find<T>(model.GetHashCode(),false);
                    
                    if(oldRecord != null)
                    {
                        da.Update(model);
                    }
                    else
                    {
                        da.Insert(model);
                    }

                    
                    return model;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return model;
            }
        }

        public T Insert<T>(T model) where T : class
        {
            using (var da = new DataAccess())
            {
                
                da.Insert(model);
                return model;
            }
        }

        public T Find<T>(int pk, bool whithChildren) where T : class
        {
            using (var da = new DataAccess())
            {

                return da.Find<T>(pk, whithChildren);
                
            }
        }
        public T First<T>(bool whithChildren) where T : class
        {
            using (var da = new DataAccess())
            {

                return da.GetList<T>(whithChildren).FirstOrDefault();

            }
        }
        public List<T> Get<T>(bool whithChildren) where T : class
        {
            using (var da = new DataAccess())
            {

                return da.GetList<T>(whithChildren).ToList();

            }
        }
        public void Update<T>(T model) where T : class
        {
            using (var da = new DataAccess())
            {

                da.Update(model);
               
            }
        }
        public void Delete<T>(T model) where T : class
        {
            using (var da = new DataAccess())
            {

                da.Delete(model);
                
            }
        }
        public void Save<T>(List<T> list) where T : class
        {
            using (var da = new DataAccess())
            {

                foreach (var record in list)
                {
                    InsertOrUpdate(record);
                }

            }
        }
    }
}
