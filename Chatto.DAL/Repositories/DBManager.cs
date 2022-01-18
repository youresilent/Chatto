using Chatto.DAL.EF;
using Chatto.DAL.Interfaces;
using System.Data.Entity.Validation;

namespace Chatto.DAL.Repositories
{
    public class DBManager : IDBManager
    {
        public ApplicationContext DataBase { get; set; }

        public DBManager(ApplicationContext db)
        {
            DataBase = db;
        }

        public void Create<T>(T item) where T : class
        {
            try
            {
                DataBase.Set<T>().Add(item as T);
                DataBase.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var ent in ex.EntityValidationErrors)
                {
                    var entry = ent.Entry;

                    switch (entry.State)
                    {
                        case System.Data.Entity.EntityState.Added:
                            {
                                entry.State = System.Data.Entity.EntityState.Detached;
                                break;
                            }
                        case System.Data.Entity.EntityState.Modified:
                            {
                                entry.CurrentValues.SetValues(entry.OriginalValues);
                                entry.State = System.Data.Entity.EntityState.Unchanged;

                                break;
                            }
                        case System.Data.Entity.EntityState.Deleted:
                            {
                                entry.State = System.Data.Entity.EntityState.Unchanged;
                                break;
                            }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }

        public void Remove<T>(T item) where T : class
        {
            try
            {
                DataBase.Set<T>().Remove(item);
                DataBase.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var ent in ex.EntityValidationErrors)
                {
                    var entry = ent.Entry;

                    switch (entry.State)
                    {
                        case System.Data.Entity.EntityState.Added:
                            {
                                entry.State = System.Data.Entity.EntityState.Detached;
                                break;
                            }
                        case System.Data.Entity.EntityState.Modified:
                            {
                                entry.CurrentValues.SetValues(entry.OriginalValues);
                                entry.State = System.Data.Entity.EntityState.Unchanged;

                                break;
                            }
                        case System.Data.Entity.EntityState.Deleted:
                            {
                                entry.State = System.Data.Entity.EntityState.Unchanged;
                                break;
                            }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
