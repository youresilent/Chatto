using Chatto.DAL.EF;
using Chatto.DAL.Entities;
using Chatto.DAL.Interfaces;
using System;
using System.Data.Entity.Validation;
using System.Linq;

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

                throw;
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

        public void Remove<T>(T item, int optionalEntryId = -1) where T : class
        {
            try
            {
                if (optionalEntryId != -1)
                {
                    T dbEntry = DataBase.Set<T>().Find(optionalEntryId);
                    DataBase.Set<T>().Remove(dbEntry);
                }
                else
                {
                    var entryState = DataBase.Entry<T>(item).State;

                    if (entryState == System.Data.Entity.EntityState.Detached)
                    {
                        DataBase.Set<T>().Attach(item);
                    }

                    DataBase.Set<T>().Remove(item);
                }
                
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

                throw;
            }
            catch
            {
                throw;
            }
        }

        public ClientFriend FindClientFriend(Guid friendId1, Guid friendId2)
        {
            ClientFriend dbRecord;

            try
            {
                dbRecord = DataBase.ClientFriends
                    .AsNoTracking()
                    .Where(w => w.Friend_Id1 == friendId1 && w.Friend_Id2 == friendId2)
                    .FirstOrDefault();
            }
            catch
            {
                throw;
            }

            return dbRecord;
        }
    }
}
