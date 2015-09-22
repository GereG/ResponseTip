

namespace TaskDatabase
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;


    public class TaskDatabaseModel : DbContext
    {
        // Your context has been configured to use a 'TaskDatabaseModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'TaskDatabase.TaskDatabaseModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'TaskDatabaseModel' 
        // connection string in the application configuration file.
        public TaskDatabaseModel()
            : base("name=TaskDatabaseModel")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    public class ResponseTipTask
    {
        /*       internal ResponseTipTask()
               {
                   BitcoinPrice = 17.3f;
               }*/
        public int ResponseTipTaskID { get; set; }
        public string userName { get; set; }

        [Required()]
        [Display(Name = "Asked Question")]
        //[RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]
        [StringLength(30)]
        public string question { get; set; }
        [Required()]
        public SocialSiteUsers socialSiteUser;

        public string BitcoinPublicAdress { get; set; }

        public float BitcoinPrice { get; set; }
        public bool isQuestionPublic { get; set; }

        public TaskStatusesEnum taskStatus { get; set; }


    }

    public class TaskStatuses
    {
        public TaskStatusesEnum currentStatus;

        private TaskStatuses()
        {
            currentStatus = TaskStatusesEnum.created;

        }
    }

    public enum TaskStatusesEnum
    {
        created, notPaid, notPaid_expired, paid, questionAsked, questionAsked_expired, QuestionAnswered, AnswerValid, allPaymentsSettled, completed
    }
}