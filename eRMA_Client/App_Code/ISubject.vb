Imports Microsoft.VisualBasic

''' <summary>
''' 發行者
''' </summary>
''' <remarks></remarks>
Public Interface ISubject
    Sub registerInterest(ByVal obj As IObserver)
End Interface

''' <summary>
''' 訂閱者
''' </summary>
''' <remarks></remarks>
Public Interface IObserver
    Sub sendNotify(ByVal oHashtable As Hashtable)
End Interface



