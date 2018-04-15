using Safe_Traveling_SERVER.Scripts.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Website
{
    class Website
    {
        OpenPort OP = new OpenPort();

        public void Start()
        {
            OP.Start(_Details.WebPort);
            WebService WebService = new WebService();
            WebService.Start(OP.Use(_Details.WebPort));
        }
    }
}
