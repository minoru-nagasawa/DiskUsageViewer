using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskUsageViewer
{
    [Table("Item")]
    public class ItemPoco
    {
        /// <summary>
        /// Primary key of INTEGER Type is 64bit on SQLite.
        /// </summary>
        [Key]
        public long Id { get; set; }

        public long? ParentId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public long Size { get; set; }
        
        [Required]
        public DateTime DateTime { get; set; }

        [ForeignKey("ParentId")]
        public virtual ItemPoco Parent { get; set; }
        
        public virtual ICollection<ItemPoco> Children { get; set; }

        public string CreateInsertCommand()
        {
            return $"INSERT INTO Item ({nameof(Id)}, {nameof(ParentId)}, {nameof(Name)}, {nameof(Size)}, {nameof(DateTime)}) VALUES ({Id}, {ParentId?.ToString() ?? "null"}, '{Name.Replace("'", "''")}', {Size}, '{DateTime.ToString("yyyy-MM-dd HH:mm:ss")}');";
            //return $"INSERT INTO Item ({nameof(Id)}, {nameof(ParentId)}, {nameof(Name)}, {nameof(Size)}) VALUES ({Id}, {ParentId?.ToString() ?? "null"}, '{Name.Replace("'", "''")}', {Size});";
        }
        public string CreateUpdateCommand()
        {
            return $"UPDATE Item SET {nameof(Size)} = '{Size}', {nameof(DateTime)} = '{DateTime.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE {nameof(Id)} = {Id};";
            //return $"UPDATE Item SET {nameof(Size)} = '{Size}' WHERE {nameof(Id)} = {Id};";
        }
    }
}
