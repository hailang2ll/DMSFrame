public string GetMemberVariableDeclarationStatement(ColumnSchema column)
{
    return GetMemberVariableDeclarationStatement("private", column);
}

public string GetMemberVariableDeclarationStatement(ViewColumnSchema column)
{
    return GetMemberVariableDeclarationStatement("private", column);
}

public string GetMemberVariableDeclarationStatement(ParameterSchema column)
{
    return GetMemberVariableDeclarationStatement("private", column);
}

public string GetMemberVariableDeclarationStatement(string protectionLevel, ColumnSchema column)
{
    string statement = protectionLevel + " ";
    if (column.IsForeignKeyMember)
    {
        statement += GetClassName(GetForeignKeyTable(column)) + " ";
        statement += "_" + GetCamelCaseName(GetClassName(GetForeignKeyTable(column)));
    }
    else
    {
        string _vartype = GetCSharpVariableType(column);
        statement += _vartype;
        statement += _vartype == "string" ? "" : "?";
        statement += " " + GetMemberVariableName(column);
    }
    statement += ";";
    return statement;
}

public string GetMemberVariableDeclarationStatement(string protectionLevel, ViewColumnSchema column)
{
    string statement = protectionLevel + " ";

    string _vartype = GetCSharpVariableType(column);
    statement += _vartype;
    statement += _vartype == "string" ? "" : "?";
    statement += " " + GetMemberVariableName(column);
    statement += ";";
    return statement;
}

public string GetMemberVariableDeclarationStatement(string protectionLevel, ParameterSchema column)
{
    string statement = protectionLevel + " ";
    string _vartype = GetCSharpVariableType(column);
    statement += _vartype;
    statement += _vartype == "string" ? "" : "?";
    statement += " " + GetMemberVariableName(column);
    statement += ";";
    return statement;
}

public string GetCamelCaseName(string value)
{
    return value.Substring(0, 1).ToLower() + value.Substring(1);
}

public string GetMemberVariableName(ColumnSchema column)
{
    string propertyName = GetPropertyName(column);
    string memberVariableName = "_" + GetCamelCaseName(propertyName);

    return memberVariableName;
}

public string GetMemberVariableName(ViewColumnSchema column)
{
    string propertyName = GetPropertyName(column);
    string memberVariableName = "_" + GetCamelCaseName(propertyName);
    return memberVariableName;
}
public string GetMemberVariableName(ParameterSchema column)
{
    string propertyName = GetPropertyName(column);
    string memberVariableName = "_" + GetCamelCaseName(propertyName);
    return memberVariableName;
}

public string GetPropertyName(ColumnSchema column)
{
    string propertyName = column.Name;
    if (propertyName == column.Table.Name + "Description") return "Description";
    return propertyName;
}

public string GetPropertyName(ViewColumnSchema column)
{
    string propertyName = column.Name;

    if (propertyName == column.Name + "Name") return "Name";
    if (propertyName == column.Name + "Description") return "Description";
    //if (propertyName.EndsWith("TypeCode")) propertyName = propertyName.Substring(0, propertyName.Length - 4);
    return propertyName;
}
public string GetPropertyName(ParameterSchema column)
{
    string propertyName = column.Name.Replace("@", "");
    return propertyName;
}

public string GetMemberVariableDefaultValue(ParameterSchema column)
{
    String strDefault = column.ExtendedProperties["CS_Default"].Value.ToString();
    if (strDefault != "")
    {
        switch (column.DataType)
        {
            case DbType.Byte:
            case DbType.Currency:
            case DbType.Decimal:
            case DbType.Double:
            case DbType.Int16:
            case DbType.Int32:
            case DbType.Int64:
            case DbType.SByte:
            case DbType.Single:
            case DbType.UInt16:
            case DbType.UInt32:
            case DbType.UInt64:
                {
                    return " 参数默认值: " + strDefault;
                }
            case DbType.AnsiString:
            case DbType.AnsiStringFixedLength:
            case DbType.String:
            case DbType.StringFixedLength:
                {
                    return " 参数默认值: \"" + strDefault + "\"";
                }
            default:
                return "";
        }
    }
    return strDefault;
}
public string GetMemberVariableDefaultValue(ViewColumnSchema column)
{
    /*
    String strDefault = column.ExtendedProperties["CS_Default"].Value.ToString();

    if (strDefault != "")
    {
        switch (column.DataType)
        {
            case DbType.Byte:
            case DbType.Currency:
            case DbType.Decimal:
            case DbType.Double:
            case DbType.Int16:
            case DbType.Int32:
            case DbType.Int64:
            case DbType.SByte:
            case DbType.Single:
            case DbType.UInt16:
            case DbType.UInt32:
            case DbType.UInt64:
            {
                return strDefault.Substring(2, strDefault.Length-4);
            }
            case DbType.AnsiString:
            case DbType.AnsiStringFixedLength:
            case DbType.String:
            case DbType.StringFixedLength:
            {
                return "\""+strDefault.Substring(2, strDefault.Length-4)+"\"";
            }
            case DbType.Boolean: 
            {
                if (strDefault.Substring(2, 1) != "0")
                  return "true";
                else
                  return "false";	
            }
            case DbType.Guid: 
            {
                return "Guid.Empty";	
            }
        }
    }
    */

    switch (column.DataType)
    {
        case DbType.Byte:
            {
                return "byte.MinValue";
            }
        case DbType.Decimal:
        case DbType.Currency:
            {
                return "decimal.MinValue";
            }
        case DbType.Double:
            {
                return "double.MinValue";
            }
        case DbType.Int16:
            {
                return "short.MinValue";
            }
        case DbType.Int32:
            {
                return "int.MinValue";
            }
        case DbType.Int64:
            {
                return "long.MinValue";
            }
        case DbType.Single:
            {
                return "float.MinValue";
            }
        case DbType.Guid:
            {
                return "Guid.Empty";
            }
        case DbType.AnsiString:
        case DbType.AnsiStringFixedLength:
        case DbType.String:
        case DbType.StringFixedLength:
            {
                return "null";
            }
        default:
            {
                return "";
            }
    }
}

public string GetMemberVariableDefaultValue(ColumnSchema column)
{
    /*
    String strDefault = column.ExtendedProperties["CS_Default"].Value.ToString();
    if (strDefault != "")
    {
        switch (column.DataType)
        {
            case DbType.Byte:
            case DbType.Currency:
            case DbType.Decimal:
            case DbType.Double:
            case DbType.Int16:
            case DbType.Int32:
            case DbType.Int64:
            case DbType.SByte:
            case DbType.Single:
            case DbType.UInt16:
            case DbType.UInt32:
            case DbType.UInt64:
            {
                return strDefault.Substring(2, strDefault.Length-4);
            }
            case DbType.AnsiString:
            case DbType.AnsiStringFixedLength:
            case DbType.String:
            case DbType.StringFixedLength:
            {
                return "\""+strDefault.Substring(2, strDefault.Length-4)+"\"";
            }
            case DbType.Boolean: 
            {
                if (strDefault.Substring(2, 1) != "0")
                  return "true";
                else
                  return "false";	
            }
            case DbType.Guid: 
            {
                return "Guid.Empty";	
            }
        }
    }
    */

    switch (column.DataType)
    {
        case DbType.Byte:
            {
                return "byte.MinValue";
            }
        case DbType.Decimal:
        case DbType.Currency:
            {
                return "decimal.MinValue";
            }
        case DbType.Double:
            {
                return "double.MinValue";
            }
        case DbType.Int16:
            {
                return "short.MinValue";
            }
        case DbType.Int32:
            {
                return "int.MinValue";
            }
        case DbType.Int64:
            {
                return "long.MinValue";
            }
        case DbType.Single:
            {
                return "float.MinValue";
            }
        case DbType.Guid:
            {
                return "Guid.Empty";
            }
        case DbType.AnsiString:
        case DbType.AnsiStringFixedLength:
        case DbType.String:
        case DbType.StringFixedLength:
            {
                return "null";
            }
        default:
            {
                return "";
            }
    }
}
        
public string GetCSharpVariableType(ColumnSchema column)
{
    //if (column.Name.EndsWith("TypeCode")) return column.Name;

    switch (column.DataType)
    {
        case DbType.AnsiString: return "string";
        case DbType.AnsiStringFixedLength: return "string";
        case DbType.Binary: return "byte[]";
        case DbType.Boolean: return "bool";
        case DbType.Byte: return "byte";
        case DbType.Currency: return "decimal";
        case DbType.Date: return "DateTime";
        case DbType.DateTime: return "DateTime";
        case DbType.Decimal: return "decimal";
        case DbType.Double: return "double";
        case DbType.Guid: return "Guid";
        case DbType.Int16: return "short";
        case DbType.Int32: return "int";
        case DbType.Int64: return "long";
        case DbType.Object: return "object";
        case DbType.SByte: return "bool";
        case DbType.Single: return "float";
        case DbType.String: return "string";
        case DbType.StringFixedLength: return "string";
        case DbType.Time: return "TimeSpan";
        case DbType.UInt16: return "ushort";
        case DbType.UInt32: return "uint";
        case DbType.UInt64: return "ulong";
        case DbType.VarNumeric: return "decimal";
        default:
            {
                return "__UNKNOWN__" + column.NativeType;
            }
    }
}

public string GetCSharpVariableType(ViewColumnSchema column)
{
    //if (column.Name.EndsWith("TypeCode")) return column.Name;

    switch (column.DataType)
    {
        case DbType.AnsiString: return "string";
        case DbType.AnsiStringFixedLength: return "string";
        case DbType.Binary: return "byte[]";
        case DbType.Boolean: return "bool";
        case DbType.Byte: return "byte";
        case DbType.Currency: return "decimal";
        case DbType.Date: return "DateTime";
        case DbType.DateTime: return "DateTime";
        case DbType.Decimal: return "decimal";
        case DbType.Double: return "double";
        case DbType.Guid: return "Guid";
        case DbType.Int16: return "short";
        case DbType.Int32: return "int";
        case DbType.Int64: return "long";
        case DbType.Object: return "object";
        case DbType.SByte: return "sbyte";
        case DbType.Single: return "float";
        case DbType.String: return "string";
        case DbType.StringFixedLength: return "string";
        case DbType.Time: return "TimeSpan";
        case DbType.UInt16: return "ushort";
        case DbType.UInt32: return "uint";
        case DbType.UInt64: return "ulong";
        case DbType.VarNumeric: return "decimal";
        default:
            {
                return "__UNKNOWN__" + column.NativeType;
            }
    }
}
public string GetCSharpVariableType(ParameterSchema column)
{
    switch (column.DataType)
    {
        case DbType.AnsiString: return "string";
        case DbType.AnsiStringFixedLength: return "string";
        case DbType.Binary: return "byte[]";
        case DbType.Boolean: return "bool";
        case DbType.Byte: return "byte";
        case DbType.Currency: return "decimal";
        case DbType.Date: return "DateTime";
        case DbType.DateTime: return "DateTime";
        case DbType.Decimal: return "decimal";
        case DbType.Double: return "double";
        case DbType.Guid: return "Guid";
        case DbType.Int16: return "short";
        case DbType.Int32: return "int";
        case DbType.Int64: return "long";
        case DbType.Object: return "object";
        case DbType.SByte: return "bool";
        case DbType.Single: return "float";
        case DbType.String: return "string";
        case DbType.StringFixedLength: return "string";
        case DbType.Time: return "TimeSpan";
        case DbType.UInt16: return "ushort";
        case DbType.UInt32: return "uint";
        case DbType.UInt64: return "ulong";
        case DbType.VarNumeric: return "decimal";
        default:
            {
                return "__UNKNOWN__" + column.NativeType;
            }
    }
}
public string GetReaderMethod(ColumnSchema column)
{
    switch (column.DataType)
    {
        case DbType.Byte:
            {
                return "GetByte";
            }
        case DbType.Int16:
            {
                return "GetInt16";
            }
        case DbType.Int32:
            {
                return "GetInt32";
            }
        case DbType.Int64:
            {
                return "GetInt64";
            }
        case DbType.AnsiStringFixedLength:
        case DbType.AnsiString:
        case DbType.String:
        case DbType.StringFixedLength:
            {
                return "GetString";
            }
        case DbType.Boolean:
            {
                return "GetBoolean";
            }
        case DbType.Guid:
            {
                return "GetGuid";
            }
        case DbType.Currency:
        case DbType.Decimal:
            {
                return "GetDecimal";
            }
        case DbType.DateTime:
        case DbType.Date:
            {
                return "GetDateTime";
            }
        default:
            {
                return "__SQL__" + column.DataType;
            }
    }
}

public string GetClassName(CommandSchema table)
{
    System.String tblName = table.Name.Replace(".", "");
    return tblName.Trim();
}
public string GetClassName(TableSchema table)
{
    System.String tblName = table.Name.Replace(".", "");
    return tblName.Trim();
}
public string GetDataBaseName(TableSchema table)
{
    System.String tblName = table.Database.Name.Replace(".", "");
    return tblName.Trim();
}
public string GetClassName(ViewSchema table)
{
    System.String tblName = table.Name.Replace(".", "");
    return tblName.Trim();
}

public string GetPrimaryKeyName(TableSchema table)
{
    if (table.PrimaryKey.MemberColumns.Count > 0)
    {
        for (int i = 0; i < table.PrimaryKey.MemberColumns.Count; i++)
        {
            ColumnSchema column = table.PrimaryKey.MemberColumns[i];
            if ((bool)column.ExtendedProperties["CS_IsIdentity"].Value)
                return GetPropertyName(column);
        }
        return GetPropertyName(table.PrimaryKey.MemberColumns[0]);
    }
    else return "";
}

public TableSchema GetForeignKeyTable(ColumnSchema column)
{
    TableSchema primaryTable = column.Table;
    foreach (TableKeySchema key in primaryTable.ForeignKeys)
    {
        if (key.ForeignKeyMemberColumns[0] == column)
        {
            return key.PrimaryKeyTable;
        }
    }
    throw new ApplicationException("Unable to locate primary table for foreign keys " + column.Name);
}
public string GetSqlDbType(ColumnSchema column)
{
    switch (column.NativeType)
    {
        case "bigint": return "BigInt";
        case "binary": return "Binary";
        case "bit": return "Bit";
        case "char": return "Char";
        case "datetime": return "DateTime";
        case "decimal": return "Decimal";
        case "float": return "Float";
        case "image": return "Image";
        case "int": return "Int";
        case "money": return "Money";
        case "nchar": return "NChar";
        case "ntext": return "NText";
        case "numeric": return "Decimal";
        case "nvarchar": return "NVarChar";
        case "real": return "Real";
        case "smalldatetime": return "SmallDateTime";
        case "smallint": return "SmallInt";
        case "smallmoney": return "SmallMoney";
        case "sql_variant": return "Variant";
        case "sysname": return "NChar";
        case "text": return "Text";
        case "timestamp": return "Timestamp";
        case "tinyint": return "TinyInt";
        case "uniqueidentifier": return "UniqueIdentifier";
        case "varbinary": return "VarBinary";
        case "varchar": return "VarChar";
        default: return "__UNKNOWN__" + column.NativeType;
    }
}
public string GetPrimaryKeyType(TableSchema table)
{
    if (table.PrimaryKey != null)
    {
        if (table.PrimaryKey.MemberColumns.Count == 1)
        {
            return GetCSharpVariableType(table.PrimaryKey.MemberColumns[0]);
        }
        else
        {
            throw new ApplicationException("This template will not work on primary keys with more than one member column.");
        }
    }
    else
    {
        throw new ApplicationException("This template will only work on tables with a primary key.");
    }
}
public ColumnSchema GetPrimaryKey(TableSchema table)
{
    if (table.PrimaryKey != null)
    {
        if (table.PrimaryKey.MemberColumns.Count == 1)
        {
            return table.PrimaryKey.MemberColumns[0];
        }
        else
        {
            throw new ApplicationException("This template will not work on primary keys with more than one member column.");
        }
    }
    else
    {
        throw new ApplicationException("This template will only work on tables with a primary key.");
    }
}