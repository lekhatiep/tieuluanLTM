using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.RoboFlow
{
    public interface IRoboFlowService
    {
        string DetectImage(string imageName );
        string DetectURLImage(string imgURL );
    }
}
