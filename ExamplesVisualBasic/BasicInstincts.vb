' https://learn.microsoft.com/en-us/archive/msdn-magazine/2009/may/documenting-your-code-with-xml-comments
Module BasicInstincts

    ''' <summary>
    ''' Determines whether a specific registry key exists.
    ''' </summary>
    ''' <param name="regKey">Name or path of the registry key.</param>
    ''' <returns>True if the registry key exists; otherwise False.</returns>
    ''' <remarks>remarks</remarks>
    Sub HowToInsert()
    End Sub

    ''' <summary>
    ''' After restarting, the XML comments automatically inserted above Function will include an author element instead of remarks:
    ''' </summary>
    ''' <param name="regKey"></param>
    ''' <returns></returns>
    ''' <author>Olivier</author>
    Sub CustomizedXml()
    End Sub
End Module
