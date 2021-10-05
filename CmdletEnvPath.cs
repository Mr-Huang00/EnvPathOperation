using System;
using System.Collections.Generic;
using System.Management.Automation;
using EnvPath;

namespace CmdletEnvPath
{
    /// <summary>
    /// Add path to Environment Path Variable 
    /// </summary>
    [Cmdlet(VerbsCommon.Add, "EnvPath",HelpUri ="https://www.acfun.cn")]
    [OutputType(typeof(List<string>))]
    public class AddEnvPath : Cmdlet
    {

        [Parameter(
            Mandatory = true, 
            Position = 0, 
            ValueFromPipeline = true)]
        public string Path { get; set; }

        [Parameter]
        [Alias("Target")]
        public EnvironmentVariableTarget EnvVarTarget { get; set; }

        /// <summary>
        /// if switch,no output.
        /// </summary>
        [Parameter]
        public SwitchParameter Quiet { get; set; }

        public AddEnvPath()
            : base()
        {
            
        }
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            var envPath = EnvPaths.GetEnvPaths(EnvVarTarget);
            if(EnvPaths.ExistsInUserAndMachine(Path))
            {
                WriteObject(envPath.GetPaths());
            }
            else
            {
                if (envPath.AddPath(Path) != string.Empty)
                {
                    if (Quiet)
                        envPath.SavePaths();
                    else
                        WriteObject(envPath.SavePaths());
                }
            }
        }
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }

    /// <summary>
    /// Get All Path of specified target.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "EnvPaths")]
    [OutputType(typeof(List<string>))]
    public class GetEnvPaths : Cmdlet
    {

        [Parameter]
        [Alias("Target")]
        public EnvironmentVariableTarget EnvVarTarget { get; set; }

        public GetEnvPaths()
            : base()
        {

        }
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            WriteObject(EnvPaths.GetEnvPaths(EnvVarTarget));
        }
    }

    /// <summary>
    /// Remove path
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "EnvPath")]
    [OutputType(typeof(List<string>))]
    public class RemoveEnvPath : Cmdlet
    {

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public string Path { get; set; }

        [Parameter]
        [Alias("Target")]
        public EnvironmentVariableTarget EnvVarTarget { get; set; }

        public RemoveEnvPath()
            : base()
        {

        }
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            var envPath = EnvPaths.GetEnvPaths(EnvVarTarget);
            if (EnvPaths.ExistsInUserAndMachine(Path))
            {
                envPath.RemovePath(Path);
                envPath.SavePaths();
                WriteObject(envPath.GetPaths());
            }
            else
            {
                WriteObject(envPath.GetPaths());
            }
        }
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
