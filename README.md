
#框架说明

.DMS框架是采用LINQ的写法的一个数据库访问框架。具体使用与现有的LINQ写法差不多一致。


.WebService必须以Service结尾，后台项目继承自WebServiceFrameBase验证登录，不登录继承自WebServiceBase,
前台项目继承自WebServicePortalBase,类里面的方法名不能重载，第一个参数为BaseResult result，
这个参数是用来前台调用的。列表查询是不用验证权限，但添加，编辑，删除等操作是要加权限的。

.BaseResult返回参数说明,默认返回0 表示无错误，数据正常。500(含500)表示系统错误.500以下为自定义错误信息.
错误信息可以写入errmsg里面提示前台.

.WebService方法，可以传任意参数，原则上建议进行参数封装。参数名称与前台JS传递参数名称要一致。

.DMSTransactionScopeEntity tsEntity = new DMSTransactionScopeEntity();
new DMSTransactionScopeHandler().Update(tsEntity, ref errMsg);

#常用的写法

DMSTransactionScopeEntity tsEntity = new DMSTransactionScopeEntity();
//多表更新的写法,不能相同的表
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



