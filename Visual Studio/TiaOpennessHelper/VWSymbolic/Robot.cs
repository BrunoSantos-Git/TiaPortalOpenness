using System.Collections.Generic;

namespace TiaOpennessHelper.VWSymbolic
{
    public static class Robot
    {
        public static List<List<RobotBase>> RobBase { get; set; }
        public static List<List<RobotTecnologie>> RobTecnologies { get; set; }
        public static List<List<RobotSafeRangeMonitoring>> RobSafeRangeMonitoring { get; set; }
        public static List<List<RobotSafeOperation>> RobSafeOperations { get; set; }

        static Robot()
        {
            RobBase = new List<List<RobotBase>>();
            RobTecnologies = new List<List<RobotTecnologie>>();
            RobSafeRangeMonitoring = new List<List<RobotSafeRangeMonitoring>>();
            RobSafeOperations = new List<List<RobotSafeOperation>>();
        }
    }
}
