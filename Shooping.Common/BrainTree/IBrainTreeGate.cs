using Braintree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooping.Common.BrainTree
{
    public  interface IBrainTreeGate
    {
        IBraintreeGateway CreateGateWay();
        IBraintreeGateway GetGateway();
    }
}
