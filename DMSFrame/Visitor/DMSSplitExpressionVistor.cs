using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DMSFrame.Access;

namespace DMSFrame
{
    /// <summary>
    /// 
    /// </summary>
    public class DMSSplitExpressionVistor
    {
        /// <summary>
        /// 
        /// </summary>
        public DMSSplitExpressionVistor()
        {
            this.TableMapping = new TableMappingAttribute()
            {
                ConfigName = ConstExpression.TableConfigConfigName,
                DMSDbType = DMSDbType.MsSql,
                Name = string.Empty,
                PrimaryKey = string.Empty,
                TokenFlag = true,
            };
        }
        /// <summary>
        /// 
        /// </summary>
        internal protected StringBuilder resultSql = new StringBuilder();
        /// <summary>
        /// 
        /// </summary>
        internal protected StringBuilder resultSqlCount = new StringBuilder();
        /// <summary>
        /// 
        /// </summary>
        internal protected List<ParamInfo> ParamList = new List<ParamInfo>();
        /// <summary>
        /// 
        /// </summary>
        internal protected DMS DMSFrame { get; set; }
        /// <summary>
        /// 
        /// </summary>
        internal protected string MemberSql { get; set; }
        /// <summary>
        /// 
        /// </summary>
        internal protected TableMappingAttribute TableMapping { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public virtual void AnalyzeExpressionUpdate()
        {
            List<ParamInfo> paramList = new List<ParamInfo>();
            ApendParamInfo(this.ParamList, ref paramList);

            StringBuilder strSql = new StringBuilder();
            string ParamSql = string.Empty;
            string tableSql = this.DMSFrame.TableExpressioin.VisitExpression(this.DMSFrame.DataType, this.DMSFrame.ExcuteType, this.DMSFrame.ParamIndex, ref ParamSql, ref ParamList);
            this.DMSFrame.ParamIndex = paramList.Count;
            string tableSql2 = string.Empty;
            if (this.DMSFrame.ExcuteType == DMSExcuteType.UPDATE_WHERE)
            {
                tableSql2 = ParamSql;
            }
            ApendParamInfo(this.ParamList, ref paramList);

            this.DMSFrame.ColumnsExpressioin.TableKeys = this.DMSFrame.TableExpressioin.TableKeys;
            this.DMSFrame.WhereExpressioin.TableKeys = this.DMSFrame.TableExpressioin.TableKeys;
            this.DMSFrame.GroupByExpression.TableKeys = this.DMSFrame.TableExpressioin.TableKeys;
            this.DMSFrame.OrderByExpressioin.TableKeys = this.DMSFrame.TableExpressioin.TableKeys;
            this.DMSFrame.HavingExpression.TableKeys = this.DMSFrame.TableExpressioin.TableKeys;


            ParamSql = string.Empty;
            string columnSql = this.DMSFrame.ColumnsExpressioin.VisitExpression(this.DMSFrame.DataType, this.DMSFrame.ExcuteType, this.DMSFrame.ParamIndex, ref ParamSql, ref ParamList);
            this.MemberSql = ParamSql;
            this.DMSFrame.ParamIndex = paramList.Count;
            ApendParamInfo(this.ParamList, ref paramList);



            ParamSql = string.Empty;
            string whereSql = this.DMSFrame.WhereExpressioin.VisitExpression(this.DMSFrame.DataType, this.DMSFrame.ExcuteType, this.DMSFrame.ParamIndex, ref ParamSql, ref ParamList);
            this.DMSFrame.ParamIndex = paramList.Count;
            ApendParamInfo(this.ParamList, ref paramList);


            strSql.Append(this.DMSFrame.Provider.Update);
            strSql.Append(tableSql);

            strSql.Append(this.DMSFrame.Provider.Set);
            strSql.Append(columnSql);
            if (this.DMSFrame.ExcuteType == DMSExcuteType.UPDATE_WHERE && !string.IsNullOrEmpty(tableSql2))
            {
                strSql.Append(this.DMSFrame.Provider.From);
                strSql.Append(tableSql2);
            }
            if (!string.IsNullOrEmpty(whereSql))
            {
                strSql.Append(this.DMSFrame.Provider.Where);

                strSql.Append(whereSql);
            }

            this.BuildParameters(paramList);
            resultSql = new StringBuilder(strSql.ToString());
        }
        /// <summary>
        /// 字段
        /// </summary>
        public virtual void AnalyzeExpressionInsert()
        {
            List<ParamInfo> paramList = new List<ParamInfo>();
            ApendParamInfo(this.ParamList, ref paramList);

            StringBuilder strSql = new StringBuilder();
            string ParamSql = string.Empty;
            string tableSql = this.DMSFrame.TableExpressioin.VisitExpression(this.DMSFrame.DataType, this.DMSFrame.ExcuteType, this.DMSFrame.ParamIndex, ref ParamSql, ref ParamList);

            string tableSql2 = ParamSql;

            this.DMSFrame.ParamIndex = paramList.Count;
            this.DMSFrame.ColumnsExpressioin.TableKeys = this.DMSFrame.TableExpressioin.TableKeys;
            this.DMSFrame.WhereExpressioin.TableKeys = this.DMSFrame.TableExpressioin.TableKeys;
            this.DMSFrame.GroupByExpression.TableKeys = this.DMSFrame.TableExpressioin.TableKeys;
            this.DMSFrame.OrderByExpressioin.TableKeys = this.DMSFrame.TableExpressioin.TableKeys;
            this.DMSFrame.HavingExpression.TableKeys = this.DMSFrame.TableExpressioin.TableKeys;


            ApendParamInfo(this.ParamList, ref paramList);

            ParamSql = string.Empty;
            string columnSql = this.DMSFrame.ColumnsExpressioin.VisitExpression(this.DMSFrame.DataType, this.DMSFrame.ExcuteType, this.DMSFrame.ParamIndex, ref ParamSql, ref ParamList);
           
            this.MemberSql = ParamSql;
            this.DMSFrame.ParamIndex = paramList.Count;
            ApendParamInfo(this.ParamList, ref paramList);

            string whereSql = string.Empty;
            if (this.DMSFrame.ExcuteType == DMSExcuteType.INSERT_SELECT)
            {
                ParamSql = string.Empty;
                whereSql = this.DMSFrame.WhereExpressioin.VisitExpression(this.DMSFrame.DataType, this.DMSFrame.ExcuteType, this.DMSFrame.ParamIndex, ref ParamSql, ref ParamList);
                this.DMSFrame.ParamIndex = paramList.Count;
                ApendParamInfo(this.ParamList, ref paramList);
            }
            strSql.Append(this.DMSFrame.Provider.Insert);
            strSql.Append(tableSql);
            strSql.Append("(");
            strSql.Append(columnSql);
            strSql.Append(")");
            if (this.DMSFrame.ExcuteType == DMSExcuteType.INSERT_SELECT)
            {
                strSql.Append(this.DMSFrame.Provider.Select);
                strSql.Append(this.MemberSql);
                strSql.Append(this.DMSFrame.Provider.From);
                strSql.Append(tableSql2);
                if (!string.IsNullOrEmpty(whereSql))
                {
                    strSql.Append(this.DMSFrame.Provider.Where);
                    strSql.Append(whereSql);
                }
            }
            else
            {
                strSql.Append(this.DMSFrame.Provider.Values);
                strSql.Append("(");
                strSql.Append(this.MemberSql);
                strSql.Append(")");

            }
            this.DMSFrame.ParamIndex = paramList.Count;



            this.BuildParameters(paramList);

            resultSql = new StringBuilder(strSql.ToString());


        }


        /// <summary>
        /// 
        /// </summary>
        public virtual void AnalyzeExpressionInsertIdentity()
        {
            this.AnalyzeExpressionInsert();
            if (!string.IsNullOrEmpty(this.DMSFrame.Provider.SelectIdentity))
            {
                resultSql.Append(this.DMSFrame.Provider.SelectIdentity);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void AnalyzeExpressionDelete()
        {
            List<ParamInfo> paramList = new List<ParamInfo>();
            ApendParamInfo(this.ParamList, ref paramList);

            StringBuilder strSql = new StringBuilder();
            string ParamSql = string.Empty;
            string tableSql = this.DMSFrame.TableExpressioin.VisitExpression(this.DMSFrame.DataType, this.DMSFrame.ExcuteType, this.DMSFrame.ParamIndex, ref ParamSql, ref ParamList);
            this.DMSFrame.ParamIndex = paramList.Count;
            this.DMSFrame.ColumnsExpressioin.TableKeys = this.DMSFrame.TableExpressioin.TableKeys;
            this.DMSFrame.WhereExpressioin.TableKeys = this.DMSFrame.TableExpressioin.TableKeys;
            this.DMSFrame.GroupByExpression.TableKeys = this.DMSFrame.TableExpressioin.TableKeys;
            this.DMSFrame.OrderByExpressioin.TableKeys = this.DMSFrame.TableExpressioin.TableKeys;
            this.DMSFrame.HavingExpression.TableKeys = this.DMSFrame.TableExpressioin.TableKeys;


            ApendParamInfo(this.ParamList, ref paramList);

            ParamSql = string.Empty;
            string whereSql = this.DMSFrame.WhereExpressioin.VisitExpression(this.DMSFrame.DataType, this.DMSFrame.ExcuteType, this.DMSFrame.ParamIndex, ref ParamSql, ref ParamList);
            this.DMSFrame.ParamIndex = paramList.Count;
            ApendParamInfo(this.ParamList, ref paramList);


            strSql.Append(this.DMSFrame.Provider.Delete);
            strSql.Append(tableSql);

            if (!string.IsNullOrEmpty(whereSql))
            {
                strSql.Append(this.DMSFrame.Provider.Where);
                strSql.Append(whereSql);
            }
            this.BuildParameters(paramList);
            resultSql = new StringBuilder(strSql.ToString());

        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void AnalyzeExpressionSelect()
        {

            List<DMSTableKeys> tableKeys = this.DMSFrame.TableExpressioin.TableKeys;
            List<ParamInfo> paramList = this.ParamList;
            if (paramList == null)
            {
                paramList = new List<ParamInfo>();
            }
            if (tableKeys == null)
            {
                tableKeys = new List<DMSTableKeys>();
            }
            else
            {
                this.DMSFrame.TableExpressioin.TableKeys = tableKeys;
            }
            this.DMSFrame.ParamIndex = paramList.Count;
            StringBuilder strSql = new StringBuilder();
            string ParamSql = string.Empty;
            string tableSql = this.DMSFrame.TableExpressioin.VisitExpression(this.DMSFrame.DataType, this.DMSFrame.ExcuteType, this.DMSFrame.ParamIndex, ref ParamSql, ref ParamList);
            
            if (this.DMSFrame.TableExpressioin.TableKeys.Count > 0)
            {
                tableKeys.AddRange(this.DMSFrame.TableExpressioin.TableKeys);
            }
            if (this.ParamList.Count > 0)
            {
                paramList.AddRange(this.ParamList);
            }
            if (tableKeys.Count != this.DMSFrame.TableExpressioin.TableKeys.Count)
            {
                DMSTableKeys lastTableKey = tableKeys.Where(q => q.TableName == this.DMSFrame.CurrentType.AssemblyQualifiedName).LastOrDefault();
                if (lastTableKey != null)
                {
                    resultSql.Append(this.DMSFrame.Provider.As);
                    resultSql.Append(lastTableKey.TableSpecialName);
                }
            }
            this.DMSFrame.ParamIndex = paramList.Count;
            this.DMSFrame.ColumnsExpressioin.TableKeys = tableKeys;
            this.DMSFrame.WhereExpressioin.TableKeys = tableKeys;
            this.DMSFrame.GroupByExpression.TableKeys = tableKeys;
            this.DMSFrame.OrderByExpressioin.TableKeys = tableKeys;
            this.DMSFrame.HavingExpression.TableKeys = tableKeys;

            ParamSql = string.Empty;
            string columnSql = this.DMSFrame.ColumnsExpressioin.VisitExpression(this.DMSFrame.DataType, this.DMSFrame.ExcuteType, this.DMSFrame.ParamIndex, ref ParamSql, ref ParamList);
            this.MemberSql = ParamSql;
            this.DMSFrame.ParamIndex = paramList.Count;
            if (this.ParamList.Count > 0)
            {
                paramList.AddRange(this.ParamList);
            }

            ParamSql = string.Empty;
            string whereSql = this.DMSFrame.WhereExpressioin.VisitExpression(this.DMSFrame.DataType, this.DMSFrame.ExcuteType, this.DMSFrame.ParamIndex, ref ParamSql, ref ParamList);
            this.DMSFrame.ParamIndex = paramList.Count;
            if (this.ParamList.Count > 0)
            {
                paramList.AddRange(this.ParamList);
            }


            if (this.resultSql.Length == 0)
            {
                strSql.Append(this.DMSFrame.Provider.Select);
                if (this.DMSFrame.TableExpressioin.DistinctFlag)
                {
                    strSql.Append(this.DMSFrame.Provider.Distinct);
                    this.DMSFrame.TableExpressioin.DistinctFlag = false;
                }
                strSql.Append(columnSql);
                strSql.Append(this.DMSFrame.Provider.From);
            }
            strSql.Append(tableSql);
            if (!string.IsNullOrEmpty(whereSql))
            {
                if (!tableSql.Trim().EndsWith(this.DMSFrame.Provider.On.Trim()))
                {
                    strSql.Append(this.DMSFrame.Provider.Where);
                }
                strSql.Append(whereSql);
            }

            string groupby = this.DMSFrame.GroupByExpression.VisitExpression(this.DMSFrame.DataType, this.DMSFrame.ExcuteType, this.DMSFrame.ParamIndex, ref ParamSql, ref ParamList);
            this.DMSFrame.ParamIndex = paramList.Count;
            if (this.ParamList.Count > 0)
            {
                paramList.AddRange(this.ParamList);
            }

            string orderSql = this.DMSFrame.OrderByExpressioin.VisitExpression(this.DMSFrame.DataType, this.DMSFrame.ExcuteType, this.DMSFrame.ParamIndex, ref ParamSql, ref ParamList);
            this.DMSFrame.ParamIndex = paramList.Count;
            if (this.ParamList.Count > 0)
            {
                paramList.AddRange(this.ParamList);
            }

            string having = this.DMSFrame.HavingExpression.VisitExpression(this.DMSFrame.DataType, this.DMSFrame.ExcuteType, this.DMSFrame.ParamIndex, ref ParamSql, ref ParamList);
            this.DMSFrame.ParamIndex = paramList.Count;
            if (this.ParamList.Count > 0)
            {
                paramList.AddRange(this.ParamList);
            }

            if (resultSql.Length == 0)
            {
                if (!string.IsNullOrEmpty(groupby))
                {
                    strSql.Append(this.DMSFrame.Provider.GroupBy + " ");
                    strSql.Append(groupby);
                }
                if (!string.IsNullOrEmpty(having))
                {
                    strSql.Append(this.DMSFrame.Provider.Having + " ");
                    strSql.Append(having);
                }
                if (!string.IsNullOrEmpty(orderSql))
                {
                    strSql.Append(this.DMSFrame.Provider.OrderBy + " ");
                    strSql.Append(orderSql);
                }
                resultSql = new StringBuilder(strSql.ToString());
            }
            else
            {
                resultSql.Append(" " + strSql.ToString());
                strSql = new StringBuilder();
                strSql.Append(this.DMSFrame.Provider.Select);
                if (this.DMSFrame.TableExpressioin.DistinctFlag)
                {
                    strSql.Append(this.DMSFrame.Provider.Distinct);
                    this.DMSFrame.TableExpressioin.DistinctFlag = false;
                }
                strSql.Append(columnSql);
                strSql.Append(this.DMSFrame.Provider.From);
                resultSql.Insert(0, strSql.ToString());
                if (!string.IsNullOrEmpty(groupby))
                {
                    resultSql.Append(this.DMSFrame.Provider.GroupBy + " ");
                    resultSql.Append(groupby);
                }
                if (!string.IsNullOrEmpty(having))
                {
                    resultSql.Append(this.DMSFrame.Provider.Having + " ");
                    resultSql.Append(having);
                }
                if (!string.IsNullOrEmpty(orderSql))
                {
                    resultSql.Append(this.DMSFrame.Provider.OrderBy + " ");
                    resultSql.Append(orderSql);
                }
            }

            //分页需求
            if (this.DMSFrame.OrderByExpressioin.SplitPagerFlag && this.DMSFrame.OrderByExpressioin.PageIndex > 0 && this.DMSFrame.OrderByExpressioin.PageSize > 0)
            {
                DMSTableKeys key = tableKeys.LastOrDefault();
                if (key == null)
                {
                    throw new DMSFrameException("tableKeys is null");
                }
                string tableKey = key.TableSpecialName;
                if (this.TableMapping.TokenFlag == true)
                {
                    tableKey = this.DMSFrame.Provider.BuildColumnName(tableKey);
                }
                this.AnalyzeExpressionPager(this.DMSFrame.OrderByExpressioin.SplitPagerFlag, this.DMSFrame.OrderByExpressioin.PageIndex, this.DMSFrame.OrderByExpressioin.PageSize, tableKey, orderSql);
                this.DMSFrame.OrderByExpressioin.SplitPagerFlag = false;
#if DEBUG
                System.Diagnostics.Debug.WriteLine(this.DMSFrame.SplitExpression.resultSqlCount.ToString());
#endif
            }
            this.BuildParameters(paramList);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SplitPagerFlag"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="tableKey"></param>
        /// <param name="orderSql"></param>
        public virtual void AnalyzeExpressionPager(bool SplitPagerFlag, int PageIndex, int PageSize, string tableKey, string orderSql)
        {
            StringBuilder tempSql = new StringBuilder(resultSql.ToString());
            resultSqlCount = new StringBuilder();
            if (SplitPagerFlag && PageIndex != 1)
            {
                DMSFrameException.ThrowIfNull(string.IsNullOrEmpty(orderSql), "分页必须包含排序字段！");
            }
            if (SplitPagerFlag)
            {
                bool flag = this.DMSFrame.Provider.ConstructPageSplitableSelectStatementForCount(resultSql.ToString(), ref resultSqlCount);
                if (!flag)
                {
                    throw new DMSFrameException("获取Count查询出错！");
                }
            }
            string splitSql;
            if (PageIndex == 1)
            {
                bool flag = this.DMSFrame.Provider.ConstructPageSplitableSelectStatementForFirstPage(resultSql.ToString(), PageSize, out splitSql);
                if (!flag)
                {
                    throw new DMSFrameException("获取TOP查询出错！");
                }
                this.resultSql = new StringBuilder(splitSql);
            }
            else
            {
                bool flag = this.DMSFrame.Provider.ConstructPageSplitableSelectStatement(resultSql.ToString(), this.MemberSql, PageIndex, PageSize, tableKey, out splitSql);
                if (!flag)
                {
                    throw new DMSFrameException("获取TOP查询出错！");
                }
                this.resultSql = new StringBuilder(splitSql);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableSpecialName"></param>
        /// <param name="resultType"></param>
        public virtual void AnalyzeExpressionSelectPacker(string tableSpecialName, Type resultType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            resultSql.Insert(0, "(");
            resultSql.Append(")");

            DMSTableKeys lastTableKeys = new DMSTableKeys()
            {
                TableName = resultType.AssemblyQualifiedName,
                AssemblyQualifiedName = resultType.AssemblyQualifiedName,
                TableSpecialName = tableSpecialName,
            };
            this.DMSFrame.TableExpressioin.TableKeys.Add(lastTableKeys);

            this.Clean();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clean()
        {
            if (this.DMSFrame != null)
            {
                this.DMSFrame.TableExpressioin.Expression = null;
                this.DMSFrame.ColumnsExpressioin.Expression = null;
                this.DMSFrame.WhereExpressioin.Expression = null;
                this.DMSFrame.OrderByExpressioin.Expression = null;
                this.DMSFrame.GroupByExpression.Expression = null;
                this.DMSFrame.HavingExpression.Expression = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParamList"></param>
        /// <param name="paramList"></param>
        protected void ApendParamInfo(IEnumerable<ParamInfo> ParamList, ref List<ParamInfo> paramList)
        {
            if (ParamList != null && ParamList.Count() > 0)
            {
                paramList.AddRange(ParamList);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramList"></param>
        protected void BuildParameters(List<ParamInfo> paramList)
        {
            this.ParamList = new List<ParamInfo>(paramList);
            foreach (var item in ParamList)
            {
                this.DMSFrame.dynamicParameters.Add(item.Name, item.MemberName, item.Value, item.DbType, item.ParameterDirection, item.Size);
            }
        }
    }
}
