using System;

public class ProtobufDataBase : IOriginModel {
	public byte[] originData ;

    private ErrorMsg errorMsgInfo = null;

    private Type type;
    public Type GetObjectType() {
        return type;
    }

    public byte[] Data {
        get { return originData; }
    }

    public ProtobufDataBase() {
        Init();
    }

    public ProtobufDataBase(object instance) {
        Init();
//		SerializeData (instance);
        type = instance.GetType();
    }

    void Init() {
        errorMsgInfo = new ErrorMsg();
    }

    /// <summary>
    /// set data
    /// </summary>
    /// <returns>true</returns>
    /// <c>false</c>
    /// <param name="instance">Instance.</param>
    public ErrorMsg SerializeData(object instance) {
        originData = ProtobufSerializer.SerializeToBytes(instance);
//		LogHelper.LogError ("SerializeData origindata : " + originData.Length);
        return Dipose(originData, instance.GetType().ToString());
    }

    /// <summary>
    /// get data
    /// </summary>
    /// <returns>The data.</returns>
    public object DeserializeData() {
        LogHelper.LogWarning("DeserializeData origindata : " + originData.Length);
        return ProtobufSerializer.ParseFormBytes(originData, type);
    }


	public static T DeserializeData<T>(byte[] data) {
		return ProtobufSerializer.ParseFormBytes<T>(data);
	}

    /// <summary>
    /// get data by T
    /// </summary>
    /// <returns>The data.</returns>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public T DeserializeData<T>() {
        LogHelper.LogWarning("DeserializeData origindata : " + originData.Length);
        return ProtobufSerializer.ParseFormBytes<T>(originData);
    }

    /// <summary>
    /// serialize class to byte array
    /// </summary>
    /// <returns>The object.</returns>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public byte[] SerializeObject<T>(T instance) {
        return ProtobufSerializer.SerializeToBytes<T>(instance);
    }

    /// <summary>
    /// check data vaild
    /// </summary>
    /// <param name="data">Data.</param>
    protected ErrorMsg Validate(byte[] data) {
        object obj = ProtobufSerializer.ParseFormBytes(data, type);

        return Dipose(obj, "");
    }


    ErrorMsg Dipose(object ins, string name) {
        if (ins == null) {
            errorMsgInfo.Code = (int)ErrorCode.ILLEGAL_DATA;
            LogHelper.Log(" sorry ! " + name + " is illegal instance !");
        }
        else {
            errorMsgInfo.Code = (int)ErrorCode.SUCCESS;
            LogHelper.Log("congratulations ! " + name + " instance  success !");
        }

        return errorMsgInfo;
    }
}