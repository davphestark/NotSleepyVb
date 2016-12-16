Public Class NotSleepy
    Private Delegate Sub ShakeItDelegate(ByVal iPix As Integer)
    Private QueueProcessorThread As Threading.Thread
    Private QueueProcessorCancel As Boolean

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        StopQueueProcessor()
        Me.Close()
    End Sub

    Private Sub RunningToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RunningToolStripMenuItem.Click
        StartQueueProcessor()
    End Sub

    Private Sub NotSleepy_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Me.WindowState = FormWindowState.Minimized Then
            Me.Hide()
        End If
    End Sub

    Private Sub StartQueueProcessor()
        Debug.WriteLine("Starting Queue Processor...")
        QueueProcessorCancel = False
        QueueProcessorThread = New Threading.Thread(AddressOf QueueProcessor)
        QueueProcessorThread.IsBackground = True
        QueueProcessorThread.Start()
    End Sub

    Private Sub StopQueueProcessor()
        If QueueProcessorThread IsNot Nothing AndAlso QueueProcessorThread.IsAlive Then
            Debug.WriteLine("Stopping Queue Processor...")
            QueueProcessorCancel = True
            QueueProcessorThread.Interrupt()
            If Not QueueProcessorThread.Join(1000) Then
                Debug.WriteLine("Queue Processor did not respond to stop request. Aborting Queue Processor...")
                QueueProcessorThread.Abort()
            End If
        End If
    End Sub

    Private Sub QueueProcessor()
        Debug.WriteLine("Queue Processor Started:" & Now.ToString)
        Try
            Do
                Try
                    Debug.WriteLine("Processing Queue...")
                    Invoke(New ShakeItDelegate(AddressOf ShakeIt), New Object() {1})
                    Threading.Thread.Sleep(480000)
                Catch ex As Threading.ThreadInterruptedException
                End Try
            Loop Until QueueProcessorCancel
        Catch ex As Threading.ThreadAbortException
            Debug.WriteLine("Queue Processor Aborted.")
        Catch ex As Exception
            Debug.WriteLine(String.Format("An unhandled error occured: {0}", ex.Message))
        End Try
        Debug.WriteLine("Queue Processor Stopped.")
    End Sub

    Private Sub ShakeIt(ByVal iPix As Integer)
        'Me.Cursor = New Cursor(Cursor.Current.Handle)
        'Cursor.Position = New Point(Cursor.Position.X - iPix, Cursor.Position.Y)
        'Cursor.Position = New Point(Cursor.Position.X + iPix, Cursor.Position.Y)
        SendKeys.Send("{NUMLOCK}{NUMLOCK}")
    End Sub

    Private Sub StopToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StopToolStripMenuItem.Click
        StopQueueProcessor()
    End Sub
End Class
