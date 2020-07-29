﻿using System;
using System.Collections.Generic;

using Common.Models;

namespace AgentCore.Models
{
    [Serializable]
    public class AgentModule
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Developer> Developers { get; set; }
        public List<AgentCommand> Commands { get; set; }
        public bool NotifyTeamServer { get; set; } = true;
    }
}