using System.Collections.Generic;

namespace Framework.Domain.Dtos.Education
{
    public class UserMissionsDto
    {
        public MissionDto CurrentMission { get; set; }
        public List<MissionDto> OverdueMissions { get; set; }
        public List<MissionDto> CompletedMissions { get; set; }
        public List<MissionDto> UpcomingMissions { get; set; }
    }
}
