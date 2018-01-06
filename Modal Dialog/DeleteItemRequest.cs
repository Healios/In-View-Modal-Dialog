using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Interactivity.InteractionRequest;

namespace Modal_Dialog
{
    internal class DeleteItemRequest : Confirmation
    {
        public string Item { get; set; }
    }
}