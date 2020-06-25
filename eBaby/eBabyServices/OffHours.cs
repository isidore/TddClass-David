using System;

public sealed class OffHours : Hours
{
    private readonly Random m_Random = new Random();

    private OffHours()
    {
    }

    #region Hours Members

    public bool IsOffHours()
    {
        bool ret = true;
        ret = m_Random.Next()*10.0 >= 5.0 ? true : false;
        return ret;
    }

    #endregion

    public static OffHours GetInstance()
    {
        return new OffHours();
    }
}