#框架说明


.DMS框架是采用LINQ的写法的一个数据库访问框架。具体使用与现有的LINQ写法差不多一致。

#常用的写法
1、单列添加/修改/删除   

	Pro_FundCompany entity = new Pro_FundCompany()
	{
		FundKey = Guid.NewGuid(),
		FundName = param.FundName,
		FundLogo = param.FundLogo,
		FundDesc = string.IsNullOrWhiteSpace(param.FundDesc) ? string.Empty : param.FundDesc,
		FundCode = param.FundCode,
	};
	return DMS.Create<T>().InsertIdentity(entity);
	return DMS.Create<T>().Edit(entity, q=>q.id=id) > 0;
	return DMS.Create<T>().Delete(q=>q.id=id) > 0;
	
	
 返回int类型
 
2、查询语句

	基本查询：
	DMS.Create<Pro_FundCompany>().Where(q => q.FundName == param.FundName && q.DeleteFlag == false).ToList();
	
	分页查询： 
	DMS.Create<T>().Where(q => q.VestName.Like(entity.VestName)&& q => q.VestType == entity.VestType)
					.OrderBy(q => q.OrderBy(q.CreateTime.Desc()))
					.Pager(entity.PageIndex, entity.PageSize)
					.ToConditionResult(entity.TotalCount);
 
3、事物处理
DMSTransactionScopeEntity tsEntity = new DMSTransactionScopeEntity();

#多表更新的写法,不能相同的表
	tsEntity.EditTS<Pro_ProductSpec, Pro_ProductMST>(x => new Pro_ProductSpec()
	{
		ProductID = x.ProductID
	}, (x, y) => x.ProductKey == y.ProductKey && x.ProductKey == Guid.NewGuid(),"更新表数据库名称(默认为空)","条件表数据库名称(默认为空)");

	// Insert into .... Select 写法,where条件不能为空
	 tsEntity.AddTS<Pro_ProductSpec, Pro_ProductMST>(product => new Pro_ProductSpec()
	{
		ProductID = product.ProductID
	}, x => x.ProductID == 1000);


	DMS.Create<Pro_ProductSpec, Pro_ProductMST>.InsertSelect(product => new Pro_ProductSpec()
	{
		ProductID = product.ProductID
	}, x => x.ProductID == 1000);

#事物统一处理
	string errMsg = "";
	if (!new DMSTransactionScopeHandler().Update(tsEntity, ref errMsg))
	{
		result.errno = 2;
		result.errmsg = "失败," + errMsg;
		return;
	}

