using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITClientServer
{
    [DataObject(true)]
   public class CoordinatorCollection : List<Coordinator>
    {
        public CoordinatorCollection coordinatorCollection { get { return coordinatorCollection; } set { coordinatorCollection = value; } }
        public CoordinatorCollection()
        {
            DataAccessLayer DAL = new DataAccessLayer(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);

            DataTable dtCoordinator = DAL.RunMySQLDataTable("select * from employee, login inner join coordinator where employee.employeeID = coordinator.coordinatorID AND employee.employeeID = login.employeeID AND employee.status = 1");

            foreach (DataRow drCoordinator in dtCoordinator.Rows)
            {
                Coordinator newCoordinator = new Coordinator(drCoordinator);
                this.Add(newCoordinator);
            }
        }

    }
}
