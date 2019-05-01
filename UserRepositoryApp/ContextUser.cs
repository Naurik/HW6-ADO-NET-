namespace UserRepositoryApp
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ContextUser : DbContext
    {
        // Контекст настроен для использования строки подключения "ContextUser" из файла конфигурации  
        // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
        // "UserRepositoryApp.ContextUser" в экземпляре LocalDb. 
        // 
        // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "ContextUser" 
        // в файле конфигурации приложения.
        public ContextUser()
            : base("name=ContextUser")
        {
        }
        public DbSet<User> Users { get; set; }
        // Добавьте DbSet для каждого типа сущности, который требуется включить в модель. Дополнительные сведения 
        // о настройке и использовании модели Code First см. в статье http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}