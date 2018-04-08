using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Twendanishe.Models
{
    /// <summary>
    /// The base class containing the properties required for all the other models
    /// </summary>
    public abstract class Base
    {
        /// <summary>
        /// The ID of the model record
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The date the record was last modified.
        /// Returns the current datetime by default.
        /// </summary>
        private DateTime _dateModified;
        [DefaultValue(typeof(DateTime), "")]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateModified
        {
            get { return DateTime.Now; }
            set { _dateModified = value; }
        }
        
        private DateTime _dateCreated { get; set; }
        /// <summary>
        /// The date the record was created
        /// </summary>
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue(typeof(DateTime), "")]
        public DateTime DateCreated
        {
            get {
                if (_dateCreated == null)
                {
                    return DateTime.Now;
                }
                else
                {
                    return _dateCreated;
                }
            }
            set { _dateCreated = value; }
        }

    }
}
