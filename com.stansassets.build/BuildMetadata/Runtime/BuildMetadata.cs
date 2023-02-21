using System;
using StansAssets.Foundation.Extensions;
using UnityEngine;

namespace StansAssets.Build.Meta
{
    /// <summary>
    /// Metadata about current build
    /// </summary>
    public class BuildMetadata : ScriptableObject
    {
        [SerializeField]
        long m_BuildTime;

        [SerializeField]
        double m_CommitTimeUnix;

        DateTime m_BuildTimeDate = DateTime.MinValue;
        DateTime m_CommitTimeDate = DateTime.MinValue;

        /// <summary>
        /// True if working copy had local changes during the build.
        /// </summary>
        [field: SerializeField]
        public bool HasChangesInWorkingCopy { get; internal set; }

        /// <summary>
        /// Repository branch name.
        /// </summary>
        [field: SerializeField]
        public string BranchName { get; internal set; }

        /// <summary>
        /// Hash string which identifies this commit
        /// </summary>
        [field: SerializeField]
        public string CommitHash { get; internal set; }

        /// <summary>
        /// Short version of a hash string which identifies this commit.
        /// </summary>
        [field: SerializeField]
        public string CommitShortHash { get; internal set; }

        /// <summary>
        /// 7 symbols hash like the one used by GitHub UI.
        /// </summary>
        [field: SerializeField]
        public string GitCommitHubHash { get; internal set; }

        /// <summary>
        /// Commit message that identifies changes.
        /// </summary>
        [field: SerializeField]
        public string CommitMessage { get; internal set; }

        /// <summary>
        /// The note left by a person who made this build.
        /// </summary>
        [field: SerializeField]
        public string Note { get; internal set; }

        /// <summary>
        /// The name of machine that was used to produce this build
        /// </summary>
        [field: SerializeField]
        public string MachineName { get; internal set; }

        /// <summary>
        /// The full Unity version string. See `InternalEditorUtility.GetFullUnityVersion()` for more details.
        /// </summary>
        [field: SerializeField]
        public string FullUnityVersion { get; internal set; }

        /// <summary>
        /// Time when build was made.
        /// To print in a nice formatted way,
        /// check an article <see href="https://www.c-sharpcorner.com/blogs/date-and-time-format-in-c-sharp-programming1">here</see>.
        /// </summary>
        public DateTime BuildTime
        {
            get
            {
                if (m_BuildTimeDate == DateTime.MinValue)
                {
                    m_BuildTimeDate = new DateTime(m_BuildTime);
                }

                return m_BuildTimeDate;
            }
        }

        /// <summary>
        /// Time when commit was made.
        /// To print in a nice formatted way,
        /// check an article <see href="https://www.c-sharpcorner.com/blogs/date-and-time-format-in-c-sharp-programming1">here</see>.
        /// </summary>
        public DateTime CommitTime
        {
            get
            {
                if (m_CommitTimeDate == DateTime.MinValue)
                {
                    m_CommitTimeDate =  UnixTimeStampToDateTime(m_CommitTimeUnix);
                }

                return m_CommitTimeDate;
            }
        }

        /// <summary>
        /// Returns application version number (Read Only).
        /// Duplicated Unity <see cref="Application.version"/>. Just for your convenience.
        /// </summary>
        public string Version => Application.version;

        /// <summary>
        /// The build number of the bundle.
        /// Only valid for iOS or Android platforms.
        /// </summary>
        [field: SerializeField]
        public int BuildNumber { get; internal set; }

        internal void SetBuildTime(long ticks)
        {
            m_BuildTime = ticks;
        }

        internal void SetCommitTime(double unixTime)
        {
            m_CommitTimeUnix = unixTime;
        }
        
        /// <summary>
        /// Converts unix timestamp to <see cref="DateTime"/> with high precision.
        /// </summary>
        /// <param name="unixTimeStamp">Unix timestamp.</param>
        /// <returns>DateTime object that represents the same moment in time on machine as provided Unix time.</returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
