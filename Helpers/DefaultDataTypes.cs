using Erp_ang2.Data;
using Erp_ang2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_ang2.Helpers
{
    public class DefaultDataTypes
    {
        private readonly ApplicationDbContext dbContext;

        public DefaultDataTypes(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public string GenerateDefaultData()
        {

            dbContext.Types.AddRange(
                new ProjectType()
                {
                    NameType = "Work",
                },
                new ProjectType()
                {
                    NameType = "Book",
                },
                new ProjectType()
                {
                    NameType = "Course",
                },
                new ProjectType()
                {
                    NameType = "Blog",
                },
                new ProjectType()
                {
                    NameType = "Other",
                }
             );

            dbContext.SaveChanges();

            return "Project types and admin was added to db!!!";

        }
    }
}
