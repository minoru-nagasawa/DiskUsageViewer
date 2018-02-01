Imports System.Windows.Forms
Imports System.ComponentModel
''' <summary>
''' Class to provides data for the collection event.
''' </summary>
Public Class CollectionEventArgs
    Inherits EventArgs
    <Description("Determine the event type raised by the collection.")> _
    Public Enum EventType
        OnClear
        OnClearComplete
        OnInsert
        OnInsertComplete
        OnRemove
        OnRemoveComplete
        OnSet
        OnSetComplete
    End Enum
    Dim _index As Integer = -1
    Dim _type As EventType = EventType.OnClear
    Dim _item As Object = Nothing
    Dim _oldValue As Object = Nothing
    Dim _newValue As Object = Nothing
    <Description("Create an instance of CollectionEventArgs that have no arguments.")> _
    Public Sub New(ByVal type As EventType)
        MyBase.New()
        _type = type
    End Sub
    <Description("Create an instance of CollectionEventArgs that have index and item arguments.")> _
    Public Sub New(ByVal type As EventType, ByVal index As Integer, ByVal item As Object)
        MyBase.New()
        _type = type
        _index = index
        _item = item
    End Sub
    <Description("Create an instance of CollectionEventArgs that have index, oldvalue, and newvalue arguments.")> _
    Public Sub New(ByVal type As EventType, ByVal index As Integer, ByVal oldValue As Object, ByVal newValue As Object)
        MyBase.New()
        _type = type
        _index = index
        _oldValue = oldValue
        _newValue = newValue
    End Sub
    <Description("Gets the type of the event raised.")> _
    Public ReadOnly Property Type() As EventType
        Get
            Return _type
        End Get
    End Property
    <Description("Gets the item index in the collection.")> _
    Public ReadOnly Property Index() As Integer
        Get
            Return _index
        End Get
    End Property
    <Description("Gets the item of the collection associated with the event.")> _
    Public ReadOnly Property Item() As Object
        Get
            Return _item
        End Get
    End Property
    <Description("Gets the old value of the item associated with the event.")> _
    Public ReadOnly Property OldValue() As Object
        Get
            Return _oldValue
        End Get
    End Property
    <Description("Gets the new value of the item associated with the event.")> _
    Public ReadOnly Property NewValue() As Object
        Get
            Return _newValue
        End Get
    End Property
End Class