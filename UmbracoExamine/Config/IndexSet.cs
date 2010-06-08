﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Web;
using UmbracoExamine;

namespace UmbracoExamine.Config
{
    public sealed class IndexSet : ConfigurationElement
    {

        [ConfigurationProperty("SetName", IsRequired = true, IsKey = true)]
        public string SetName
        {
            get
            {
                return (string)this["SetName"];
            }           
        }

        private string m_IndexPath = "";
        
        /// <summary>
        /// The folder path of where the lucene index is stored
        /// </summary>
        /// <remarks>
        /// This can be set at runtime but will not be persisted to the configuration file
        /// </remarks>
        [ConfigurationProperty("IndexPath", IsRequired = true, IsKey = false)]
        public string IndexPath
        {
            get
            {
                if (string.IsNullOrEmpty(m_IndexPath)) {
                    m_IndexPath = (string)this["IndexPath"];   
                }
                return m_IndexPath;
            }
            set
            {
                m_IndexPath = value;
            }
        }

        /// <summary>
        /// Set this to configure the default maximum search results for an index set.
        /// If not set, 200 is the default.
        /// </summary>
        //[ConfigurationProperty("MaxResults", IsRequired = false, IsKey = false)]
        //public int MaxResults
        //{
        //    get
        //    {
        //        if (this["MaxResults"] == null)
        //            return 200;

        //        return (int)this["MaxResults"];
        //    }            
        //}

        /// <summary>
        /// Returns the DirectoryInfo object for the index path.
        /// </summary>
        public DirectoryInfo IndexDirectory
        {
            get
            {
                //we need to de-couple the context
                if (HttpContext.Current != null)
                {
                    return new DirectoryInfo(HttpContext.Current.Server.MapPath(this.IndexPath));
                }
                else
                {
                    return new DirectoryInfo(this.IndexPath);
                }
                
            }
        }

        /// <summary>
        /// When this property is set, the indexing will only index documents that are children of this node.
        /// </summary>
        [ConfigurationProperty("IndexParentId", IsRequired = false, IsKey = false)]
        public int? IndexParentId
        {
            get
            {
                if (this["IndexParentId"] == null)
                    return null;

                return (int)this["IndexParentId"];
            }
        }

        /// <summary>
        /// The collection of node types to index, if not specified, all node types will be indexed (apart from the ones specified in the ExcludeNodeTypes collection).
        /// </summary>
        [ConfigurationCollection(typeof(IndexFieldCollection))]
        [ConfigurationProperty("IncludeNodeTypes", IsDefaultCollection = false, IsRequired = false)]
        public IndexFieldCollection IncludeNodeTypes
        {
            get
            {
                return (IndexFieldCollection)base["IncludeNodeTypes"];
            }
        }

        /// <summary>
        /// The collection of node types to not index. If specified, these node types will not be indexed.
        /// </summary>
        [ConfigurationCollection(typeof(IndexFieldCollection))]
        [ConfigurationProperty("ExcludeNodeTypes", IsDefaultCollection = false, IsRequired = false)]
        public IndexFieldCollection ExcludeNodeTypes
        {
            get
            {
                return (IndexFieldCollection)base["ExcludeNodeTypes"];
            }
        }

        /// <summary>
        /// A collection of user defined umbraco fields to index
        /// </summary>
        /// <remarks>
        /// If this property is not specified, or if it's an empty collection, the default user fields will be all user fields defined in Umbraco
        /// </remarks>
        [ConfigurationCollection(typeof(IndexFieldCollection))]
        [ConfigurationProperty("IndexUserFields", IsDefaultCollection = false, IsRequired = false)]
        public IndexFieldCollection IndexUserFields
        {
            get
            {
                return (IndexFieldCollection)base["IndexUserFields"];

                //var fields = base["IndexUserFields"] != null ? (IndexFieldCollection)base["IndexUserFields"] : new IndexFieldCollection();
                //if (fields.Count == 0)
                //{
                //    //create the defaults

                //    fields.Add(new IndexField() { Name = "id" });
                //    fields.Add(new IndexField() { Name = "version" });
                //    fields.Add(new IndexField() { Name = "parentID" });
                //    fields.Add(new IndexField() { Name = "level" });
                //    fields.Add(new IndexField() { Name = "writerID" });
                //    fields.Add(new IndexField() { Name = "creatorID" });
                //    fields.Add(new IndexField() { Name = "nodeType" });
                //    fields.Add(new IndexField() { Name = "template" });
                //    fields.Add(new IndexField() { Name = "sortOrder" });
                //    fields.Add(new IndexField() { Name = "createDate" });
                //    fields.Add(new IndexField() { Name = "updateDate" });
                //    fields.Add(new IndexField() { Name = "nodeName" });
                //    fields.Add(new IndexField() { Name = "urlName" });
                //    fields.Add(new IndexField() { Name = "writerName" });
                //    fields.Add(new IndexField() { Name = "creatorName" });
                //    fields.Add(new IndexField() { Name = "nodeTypeAlias" });
                //    fields.Add(new IndexField() { Name = "path" });
                //}

                //return fields;
            }
        }

        /// <summary>
        /// The fields umbraco values that will be indexed. i.e. id, nodeTypeAlias, writer, etc...
        /// </summary>
        /// <remarks>
        /// If this is not specified, or if it's an empty collection, the default optins will be specified:
        /// - id
        /// - version
        /// - parentID
        /// - level
        /// - writerID
        /// - creatorID
        /// - nodeType
        /// - template
        /// - sortOrder
        /// - createDate
        /// - updateDate
        /// - nodeName
        /// - urlName
        /// - writerName
        /// - creatorName
        /// - nodeTypeAlias
        /// - path
        /// </remarks>
        [ConfigurationCollection(typeof(IndexFieldCollection))]
        [ConfigurationProperty("IndexAttributeFields", IsDefaultCollection = false, IsRequired = false)]
        public IndexFieldCollection IndexAttributeFields
        {
            get
            {
                return (IndexFieldCollection)base["IndexAttributeFields"];
                //var fields = base["IndexAttributeFields"] != null ? (IndexFieldCollection)base["IndexAttributeFields"] : new IndexFieldCollection();
                //if (fields.Count == 0)
                //{
                //    //create the defaults

                //    fields.Add(new IndexField() { Name = "id" });
                //    fields.Add(new IndexField() { Name = "version" });
                //    fields.Add(new IndexField() { Name = "parentID" });
                //    fields.Add(new IndexField() { Name = "level" });
                //    fields.Add(new IndexField() { Name = "writerID" });
                //    fields.Add(new IndexField() { Name = "creatorID" });
                //    fields.Add(new IndexField() { Name = "nodeType" });
                //    fields.Add(new IndexField() { Name = "template" });
                //    fields.Add(new IndexField() { Name = "sortOrder" });
                //    fields.Add(new IndexField() { Name = "createDate" });
                //    fields.Add(new IndexField() { Name = "updateDate" });
                //    fields.Add(new IndexField() { Name = "nodeName" });
                //    fields.Add(new IndexField() { Name = "urlName" });
                //    fields.Add(new IndexField() { Name = "writerName" });
                //    fields.Add(new IndexField() { Name = "creatorName" });
                //    fields.Add(new IndexField() { Name = "nodeTypeAlias" });
                //    fields.Add(new IndexField() { Name = "path" });
                //}

                //return fields;
            }
        }


    }

    


}