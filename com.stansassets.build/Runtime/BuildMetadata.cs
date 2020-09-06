using System;
using StansAssets.Foundation.Extensions;
using UnityEngine;

namespace StansAssets.Build
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
                    m_CommitTimeDate =  DateTimeExtensions.UnixTimestampToDateTime(m_CommitTimeUnix);
                }

                return m_CommitTimeDate;
            }
        }

        internal void SetBuildTime(long ticks)
        {
            m_BuildTime = ticks;
        }

        internal void SetCommitTime(double unixTime)
        {
            m_CommitTimeUnix = unixTime;
        }
    }
}
