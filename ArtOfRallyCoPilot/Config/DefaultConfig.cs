namespace ArtOfRallyCoPilot.Config
{
    public class DefaultConfig
    {
        public static CoPilotGeneratorConfig[] CoPilotGeneratorConfig =
        {
            new CoPilotGeneratorConfig
            {
                Name = "SqR",
                Mode = CoPilotMode.ANGLE,
                DistanceSumTolerance = 60,
                MinimumValue = 65,
                MaximumValue = 95,
                WarningDistance = 0,
            },
            new CoPilotGeneratorConfig
            {
                Name = "3R",
                Mode = CoPilotMode.ANGLE,
                DistanceSumTolerance = 25,
                MinimumValue = 35,
                MaximumValue = 45,
                WarningDistance = 0,
            }
        };
    }
}
