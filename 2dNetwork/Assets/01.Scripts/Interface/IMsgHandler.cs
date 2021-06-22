using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMsgHandler
{
    void HandleMsg(string payload);
}
