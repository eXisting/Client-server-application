using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Synchronizer_MVP_pattern
{

    /*-----------------------------------------------------------------*/

    internal sealed class MainModel : Model
    {

        /*-----------------------------------------------------------------*/

        public readonly ServerSide server = new ServerSide();

        public readonly Client client = new Client();

        /*-----------------------------------------------------------------*/


    }

    /*-----------------------------------------------------------------*/

}
