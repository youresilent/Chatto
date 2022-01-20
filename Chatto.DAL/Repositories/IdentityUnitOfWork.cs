using Chatto.DAL.EF;
using Chatto.DAL.Identity;
using Chatto.DAL.Interfaces;
using System;
using System.Data.Entity.Validation;
using System.Threading.Tasks;

namespace Chatto.DAL.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private bool disposed = false;

        private readonly ApplicationContext DataBase;

        private readonly ApplicationUserManager userManager;
        private readonly ApplicationRoleManager roleManager;
        private readonly IClientManager clientManager;
        private readonly IMessageManager messageManager;
        private readonly IDBManager dbManager;

        public IdentityUnitOfWork()
        {
            DataBase = new ApplicationContext();

            userManager = new ApplicationUserManager(new CustomUserStore(DataBase));
            roleManager = new ApplicationRoleManager(new CustomRoleStore(DataBase));

            clientManager = new ClientManager(DataBase);
            messageManager = new MessageManager(DataBase);
            dbManager = new DBManager(DataBase);
        }

        public ApplicationUserManager UserManager { get { return userManager; } }

        public ApplicationRoleManager RoleManager { get { return roleManager; } }

        public IClientManager ClientManager { get { return clientManager; } }

        public IMessageManager MessageManager { get { return messageManager; } }

        public IDBManager DBManager { get { return dbManager; } }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();

                    clientManager.Dispose();
                    messageManager.Dispose();
                    dbManager.Dispose();
                }

                disposed = true;
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await DataBase.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var item in ex.EntityValidationErrors)
                {
                    var entry = item.Entry;

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
