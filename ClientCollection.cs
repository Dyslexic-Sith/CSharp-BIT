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
    public class ClientCollection : List<BITClient>
    {
        public ClientCollection clientCollection { get { return clientCollection; } set { clientCollection = value; } }
        public ClientCollection()
        {
            DataAccessLayer DAL = new DataAccessLayer(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);

            DataTable dtClients = DAL.RunMySQLDataTable("select * from Client, ClientLogin INNER JOIN ClientLocation WHERE Client.clientID = ClientLocation.clientID AND ClientLogin.clientID = Client.clientID AND Client.status = '1' ORDER BY Client.clientID");

            foreach(DataRow drClient in dtClients.Rows)
            {
                BITClient newClient = new BITClient(drClient);
                this.Add(newClient);
            }
        }
         
    }
}
