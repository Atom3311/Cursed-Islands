using UnityEngine;
static class Parameters
{
    private const string PathOfParameters = "Parameters";
    public static SO_Parameters GetParameters()
    {
        SO_Parameters parameters = Resources.Load<SO_Parameters>(PathOfParameters);
        return parameters;
    }
}