/*Made by Dante
 * 
 * Instruction: Made for changing Environment Path Variable (in registry) easily.
 * This library only use one class EnvPaths .You should modify your own EnvPaths object in program and then save the changings to let it work.
 * 
 * API list:
 *  namespace EnvPath:
 *   pulbic class EnvPaths:
 *    public EnvPaths()
 *    public EnvPaths(EnvironmentVariableTarget)
 *    
 *    public string AddPath(string)
 *    public int AddPaths(List<string>)
 *    public string RemovePath(string)
 *    public int RemovePaths(List<string>)
 *    public int RemovePaths(List<string>)
 *    public string GetPath(string)
 *    public List<string> GetPaths()
 *    public List<string> SavePaths()
 *    public override string ToString()
 *    
 *    static public EnvPaths GetEnvPaths(EnvironmentVariableTarget envTarget)
 *    static public bool ExistsInUserAndMachine(string)
 *    static public bool Validate(string)
 * 
 *    simple example:
 *     Add path to user path (registry):
 *      string path = @"e:\mydir";
 *      if(!EnvPaths.ExistsInUserAndMachine(path)) 
 *      {
 *       var myUserEnvPaths = EnvPaths.GetEnvPaths(EnvironmentVariableTarget.User);
 *       if (myUserEnvPaths.AddPath(path))
 *       {
 *        myUserEnvPaths.SavePaths();
 *       }
 *      }
 *      
 *      
 *    PS: If you want to add path to machine ,plaese remember to use it with administrator permission.
 *    
 *    Written in 7/24/2021
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Collections;


namespace EnvPath
{
    /// <summary>
    /// This class is used for easy change environment path intelligently
    /// </summary>
    public class EnvPaths
    {
        private List<string> _envPaths;
        private EnvironmentVariableTarget _envTarget;

        /// <summary>
        /// constructor of EnvPaths of current process
        /// </summary>
        public EnvPaths()
        {
            _envPaths = Environment.GetEnvironmentVariable("Path").Trim(';').Split(';').ToList<string>();
            _envTarget = EnvironmentVariableTarget.Process;
        }
        /// <summary>
        /// constructor of EnvPaths
        /// </summary>
        /// <param name="envTarget">User / Machine / Process , default is process.</param>
        public EnvPaths(EnvironmentVariableTarget envTarget)
        {
            _envPaths = Environment.GetEnvironmentVariable("Path", envTarget).Trim(';').Split(';').ToList<string>();
            _envTarget = envTarget;
        }

        /// <summary>
        /// Add path to environment path.
        /// </summary>
        /// <param name="path">Path to be added.Should be rooted and windows style.</param>
        /// <returns>If suc,return the path added;If fail,return string.Empty.</returns>
        public string AddPath(string path)
        {
            if (path == null) throw new ArgumentNullException("path is null.");
            if (!Validate(path)) throw new ArgumentException("Path is not valid.Maybe it is illegal or relative.");

            if (_envPaths.Find((e) => path.Equals(e, StringComparison.OrdinalIgnoreCase)) == null)
            {
                _envPaths.Add(path);
                return path;
            }
            return string.Empty;
        }
        /// <summary>
        /// Add paths to environment path.
        /// </summary>
        /// <param name="paths">Paths to be added.Should be rooted and windows style.</param>
        /// <returns>The count of path added successfully.</returns>
        public int AddPaths(List<string> paths)
        {
            int count = 0;
            foreach (var path in paths)
            {
                if (AddPath(path) == string.Empty) continue;
                count++;
            }
            return count;
        }
        /// <summary>
        /// Remove path from environment path.
        /// </summary>
        /// <param name="path">Path to be remove.Should be rooted and windows style.</param>
        /// <returns>If suc,return the path removed;If fail,return string.Empty.</returns>
        public string RemovePath(string path)
        {
            if (path == null) throw new ArgumentNullException("path is null.");
            if (!Validate(path)) throw new ArgumentException("Path is not valid.Maybe it is illegal or relative.");

            path = path.ToLower();
            string result = _envPaths.Find((e) => path.Equals(e, StringComparison.OrdinalIgnoreCase));
            if (result != null)
            {
                if (_envPaths.Remove(result))
                {
                    return result;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Remove paths from environment path.
        /// </summary>
        /// <param name="paths">Paths to be remove.Should be rooted and windows style.</param>
        /// <returns>The count of path removed successfully.</returns>
        public int RemovePaths(List<string> paths)
        {
            int count = 0;
            foreach (var path in paths)
            {
                if (RemovePath(path) == string.Empty) continue;
                count++;
            }
            return count;
        }
        /// <summary>
        /// Get path from environment path.Use to vertify if a path exists in environment path.
        /// </summary>
        /// <param name="path">Path to be found.Should be rooted and windows style.</param>
        /// <returns>If suc,return the path you want.If not , return string.Empty.</returns>
        public string GetPath(string path)
        {
            if (path == null) throw new ArgumentNullException("path is null.");
            if (!Validate(path)) throw new ArgumentException("Path is not valid.Maybe it is illegal or relative or unix root style.");

            path = path.ToLower();
            if (_envPaths.Find((e) => path.Equals(e, StringComparison.OrdinalIgnoreCase)) != null)
            {
                return path;
            }
            return string.Empty;
        }
        /// <summary>
        /// Get all paths from environment path.
        /// </summary>
        /// <returns>All of path from environment path variable.</returns>
        public List<string> GetPaths()
        {
            return _envPaths;
        }
        /// <summary>
        /// Let environment path to work.For process target,it will change the real environment path of process ;For User or Machine,it will change the registry of your computer then it will work for a long time.
        /// </summary>
        /// <returns>The old paths of environment path variable.</returns>
        public List<string> SavePaths()
        {
            List<string> vs = Environment.GetEnvironmentVariable("Path", _envTarget).Trim(';').Split(';').ToList();
            Environment.SetEnvironmentVariable("Path", this.ToString(), _envTarget);
            return vs;
        }


        /// <summary>
        /// Rewrite the tostring for formatting the paths to string with split character - ';'.
        /// </summary>
        /// <returns>Formatted string.</returns>
        public override string ToString()
        {
            string temp = string.Empty;
            foreach (var path in _envPaths)
            {
                temp += $"{path};";
            }
            return temp;
        }

        /// <summary>
        /// Anthor style for get an instance of EnvPaths.
        /// </summary>
        /// <param name="envTarget"></param>
        /// <returns></returns>
        static public EnvPaths GetEnvPaths(EnvironmentVariableTarget envTarget = EnvironmentVariableTarget.Process)
        {
            return new EnvPaths(envTarget);
        }

        /// <summary>
        /// Find the path in User and Machine Environment Path Variable (in registry).
        /// </summary>
        /// <param name="path"></param>
        /// <returns>If suc,return true;If fail,return false.</returns>
        static public bool ExistsInUserAndMachine(string path)
        {
            if (GetEnvPaths(EnvironmentVariableTarget.User).GetPath(path) != string.Empty || GetEnvPaths(EnvironmentVariableTarget.Machine).GetPath(path) != string.Empty)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Valiadate the path ,it should be rooted and windows style.
        /// </summary>
        /// <param name="path">If suc,return true;If fail,return false.</param>
        /// <returns></returns>
        static public bool Validate(string path)
        {
            if (path == null) throw new ArgumentNullException("path is null.");

            //refuse to use unix root style 
            if (path[0] == '/') return false;
            if (path[0] == '\\') return false;
            //refuse relative style,and check if it is illegal.
            if (!Path.IsPathRooted(path)) return false;

            return true;
        }
    }
}