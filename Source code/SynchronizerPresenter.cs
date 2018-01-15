using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace Synchronizer_MVP_pattern
{
    class SynchronizerPresenter
    {
        private readonly IView view;

        private readonly MainModel model = new MainModel();

        public SynchronizerPresenter(IView _view)
        {
            view = _view;

            #region View setup

            view.StartServerHandler += new EventHandler<EventArgs>(runServer);
            view.ShutDownServerHandler += new EventHandler<EventArgs>(StopServer);
            view.ChooseServerFolderHandler += new EventHandler<EventArgs>(selectServerFolder);
            view.FilterServerLogs += new EventHandler<EventArgs>(filterServerLogs);
            view.ClearServerLogs += new EventHandler<EventArgs>(clearServerLogs);

            view.StartClientHandler += new EventHandler<EventArgs>(connectToServer);
            view.ShutDownClientHandler += new EventHandler<EventArgs>(disconnectFromServer);
            view.ChooseClientFolderHandler += new EventHandler<EventArgs>(chooseClientFolder);
            view.FilterClientLogs += new EventHandler<EventArgs>(clientFilter);
            view.ClearClientLogs += new EventHandler<EventArgs>(clientClear);


            #endregion

            #region ServerWatcher events

            view.ServerSystemWatcherChanged += new EventHandler<FileSystemEventArgs>(changedCommandServerWatcher);
            view.ServerSystemWatcherCreated += new EventHandler<FileSystemEventArgs>(createdCommandServerWatcher);
            view.ServerSystemWatcherDeleted += new EventHandler<FileSystemEventArgs>(deletedCommandServerWatcher);
            view.ServerSystemWatcherRenamed += new EventHandler<RenamedEventArgs>(renamedCommandServerWatcher);

            #endregion

            configureModel();

            string defaultIP = model.server.Ip.ToString();

            view.SetDefaultServerIP(defaultIP);
            view.SetDefaultClientTextBoxIP(defaultIP);
        }

        private void configureModel()
        {
            #region Server configure

            model.server.InvokeMethod = new ServerSide.ControlsDelegate(
                (_newControl) => view.TakeControls()
            );
            
            model.server.Ip = Dns.GetHostEntry(model.server.Host).AddressList[0];

            AllocateMemoryForLogs(MainModel.ModelPart.Server);

            model.server.serverViewLogs = view.GetServerLogs();
            model.server.serverWatcher = view.getServerWatcher();

            #endregion

            #region Client configure

            AllocateMemoryForLogs(MainModel.ModelPart.Client);

            model.client.clientViewLogs = view.GetClientLogs();
            model.client.clientFolderDialog = view.GetClientFolderDialog();

            #endregion
        }

        #region Server implementation

        private void changedCommandServerWatcher(object sender, FileSystemEventArgs e)
        {
            FileInfo fInfo1 = new FileInfo(model.server.serverWatcher.Path + "\\" + e.Name);
            if (System.DateTime.Now == fInfo1.LastWriteTime)
            {
                Thread.Sleep(1000);
                model.server.TimeChange = fInfo1.LastWriteTime;

                string message = "Файл " + e.Name + " был изменен." + fInfo1.LastWriteTime + ";";

                log(message, MainModel.LogType.Common, MainModel.ModelPart.Server);

                model.server.Command = "chg";

                sendFile(e);
            }
        }

        private void createdCommandServerWatcher(object sender, FileSystemEventArgs e)
        {
            model.server.TimeChange = System.DateTime.Now;
            Thread.Sleep(2000);

            string message = "Файл " + e.Name + " был создан;";

            log(message, MainModel.LogType.Common, MainModel.ModelPart.Server);

            model.server.Command = "crt";

            sendFile(e);
        }

        private void deletedCommandServerWatcher(object sender, FileSystemEventArgs e)
        {
            string message = "Файл " + e.Name + " был удален;";

            log(message, MainModel.LogType.Common, MainModel.ModelPart.Server);

            model.server.Command = "del";

            deleteFile(e.Name);
        }

        private void renamedCommandServerWatcher(object sender, RenamedEventArgs e)
        {
            string message = e.OldName + " переименован в " + e.Name + ";";

            log(message, MainModel.LogType.Common, MainModel.ModelPart.Server);

            model.server.Command = "rnm";

            renameFile(e.OldName, e.Name);
        }
        
        private void configureRun()
        {
            string port = view.GetServerPort().TrimStart(new char[] { '0' });

            if (MainModel.PortRegex.Matches(port).Count > 0)
            {
                string message = "Wrong port pattern!";

                log(message, MainModel.LogType.Common, MainModel.ModelPart.Server);

                return;
            }

            model.server.StartServer = true;

            model.server.tcpListener = new TcpListener(
                    new IPEndPoint(IPAddress.Parse(model.server.Ip.ToString())
                , Convert.ToInt32(port))
            );

            model.server.listenerThread = new Thread(StartReceiving)
            {
                IsBackground = true
            };

            model.server.listenerThread.Start();

            model.server.connectionCheckerThread = new Thread(CheckConnection)
            {
                IsBackground = true
            };

            model.server.connectionCheckerThread.Start();
            
            log("Сервер запущен!;", MainModel.LogType.Common, MainModel.ModelPart.Server);
            log("Ip: " + model.server.Ip + ";", MainModel.LogType.Common, MainModel.ModelPart.Server);
            log("Port: " + port + ";", MainModel.LogType.Common, MainModel.ModelPart.Server);
        }

        private void StartReceiving()
        {
            try
            {
                model.server.tcpListener.Start();
            }
            catch (Exception e)
            {
                model.server.serverViewLogs.Items.Add(e.Message);
            }

            while (true)
            {
                try
                {
                    if (!model.server.tcpListener.Pending())
                    {
                        Thread.Sleep(500);
                        continue;
                    }

                    TcpClient client = model.server.tcpListener.AcceptTcpClient();

                    IPEndPoint IP = client.Client.RemoteEndPoint as IPEndPoint;
                    string addres = IP.Address.ToString();

                    model.server.user = new User(client, addres);

                    string message = "Клиент " + addres + " подключился;";

                    log(message, MainModel.LogType.Common, MainModel.ModelPart.Server);
                }
                catch (Exception e)
                {
                    model.server.serverViewLogs.Items.Add(e.Message);
                }
            }
        }

        private void CheckConnection()
        {
            while (true)
            {
                if (model.server.user == null || model.server.user.TcpClient == null || !model.server.user.TcpClient.Connected)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                try
                {
                    if (model.server.user.TcpClient.GetStream().Read(
                            new byte[MainModel.MAX_BYTES_BUFFER_SIZE]
                        ,   0
                        ,   MainModel.MAX_BYTES_BUFFER_SIZE) == 0
                    )                    
                        return;
                }
                catch (Exception e)
                {
                    model.server.serverViewLogs.Items.Add(e.Message);
                }

                Thread.Sleep(1000);
            }
        }

        private void runServer(object sender, EventArgs e)
        {
            if (!model.server.StartServer)
                configureRun();
        }

        private void StopServer(object sender, EventArgs e)
        {
            if (!model.server.StartServer)
                return;

            if (model.server.tcpListener != null)
            {
                model.server.tcpListener.Server.Close();
                model.server.listenerThread.Abort();
            }

            if (model.server.connectionCheckerThread != null)
                model.server.connectionCheckerThread.Abort();

            if (model.server.user != null)
                model.server.user.TcpClient.Close();

            model.server.StartServer = false;
                        
            log("Сервер остановлен!;", MainModel.LogType.Common, MainModel.ModelPart.Server);
        }

        private void selectServerFolder(object sender, EventArgs e)
        {
            FolderBrowserDialog Dialog = new FolderBrowserDialog();
            DialogResult result = Dialog.ShowDialog();

            if (result == DialogResult.OK)
                model.server.serverWatcher.Path = Dialog.SelectedPath;
        }

        private void makeFormatedLenght(ref byte[] _byteFileName, out string _info)
        {
            _info = "";
            for (int count = 0; count < 2 - _byteFileName.Length.ToString().Length; count++)
                _info += "0";

            _info += "0" + _byteFileName.Length.ToString();
        }

        private byte[] commonAction(string _name, MainModel.LogType _type)
        {
            string message = "Отправляем информацию о файле;";

            log(message, _type, MainModel.ModelPart.Server);
            
            model.server.networkStream = model.server.user.TcpClient.GetStream();

            byte[] byteSend = new byte[model.server.user.TcpClient.ReceiveBufferSize];


            FileInfo fInfo = new FileInfo(model.server.serverWatcher.Path + "\\" + _name);

            message = "Отправляем команду - " + model.server.Command + ";";

            log(message, _type, MainModel.ModelPart.Server);
            
            byte[] ByteCommand = new byte[MainModel.FIXED_BUFFER_SIZE];
            ByteCommand = System.Text.Encoding.UTF8.GetBytes(model.server.Command.ToCharArray());
            model.server.networkStream.Write(ByteCommand, 0, ByteCommand.Length);


            string FileName = fInfo.Name;


            byte[] ByteFileName = new byte[MainModel.MAX_BYTES_BUFFER_SIZE];
            ByteFileName = System.Text.Encoding.UTF8.GetBytes(FileName.ToCharArray());


            string FileLength;

            makeFormatedLenght(ref ByteFileName, out FileLength);

            message = "Отправляем длину имени - " + FileLength + ";";

            log(message, _type, MainModel.ModelPart.Server);

            byte[] ByteFileNameLength = new byte[MainModel.FIXED_BUFFER_SIZE];

            ByteFileNameLength = System.Text.Encoding.UTF8.GetBytes(FileLength.ToCharArray());
            model.server.networkStream.Write(ByteFileNameLength, 0, ByteFileNameLength.Length);

            message = "Отправляем имя - " + FileName + ";";
            log(message, _type, MainModel.ModelPart.Server);

            model.server.networkStream.Write(ByteFileName, 0, ByteFileName.Length);

            return ByteFileName;
        }

        private void sendFileToUser(FileSystemEventArgs e)
        {
            byte[] ByteFileName = commonAction(e.Name, MainModel.LogType.Create);

            model.server.fileStream = new FileStream(
                    model.server.serverWatcher.Path + "\\" + e.Name
                ,   FileMode.Open
                ,   FileAccess.Read
            );

            BinaryReader binFile = new BinaryReader(model.server.fileStream);

            FileInfo fileInfo = new FileInfo(model.server.serverWatcher.Path + "\\" + e.Name);

            string FileName = fileInfo.Name;

            long FileSize = fileInfo.Length;

            string message = "Отправляем размер файла - " + FileSize.ToString() + ";";

            log(message, MainModel.LogType.Create, MainModel.ModelPart.Server);

            byte[] ByteFileSize = new byte[MainModel.MAX_BYTES_BUFFER_SIZE];
            ByteFileSize = System.Text.Encoding.UTF8.GetBytes(FileSize.ToString().ToCharArray());

            string FileSizeLength;

            makeFormatedLenght(ref ByteFileSize, out FileSizeLength);

            message = "Отправляем длину размера - " + FileSizeLength + ";";

            log(message, MainModel.LogType.Create, MainModel.ModelPart.Server);

            byte[] ByteFileSizeLength = new byte[MainModel.FIXED_BUFFER_SIZE];

            ByteFileSizeLength = System.Text.Encoding.UTF8.GetBytes(FileSizeLength.ToCharArray());
            model.server.networkStream.Write(ByteFileSizeLength, 0, ByteFileSizeLength.Length);


            model.server.networkStream.Write(ByteFileSize, 0, ByteFileSize.Length);

            message = "Отправляем файл " + FileName + " (" + FileSize + " байт);";

            log(message, MainModel.LogType.Create, MainModel.ModelPart.Server);


            int bytesSize = 0, tempSize = 0;
            byte[] downBuffer = new byte[MainModel.MAX_BYTES_BUFFER_SIZE];

            while (true)
            {
                long contentDifference = FileSize - MainModel.MAX_BYTES_BUFFER_SIZE;
                int readBorder = downBuffer.Length;
                int additionalTempSize = MainModel.MAX_BYTES_BUFFER_SIZE;

                if (contentDifference < 0)
                {
                    readBorder = Convert.ToInt32(FileSize);
                    downBuffer = new byte[readBorder];
                    additionalTempSize = bytesSize;
                }

                bytesSize = model.server.fileStream.Read(downBuffer, 0, readBorder);
                model.server.networkStream.Write(downBuffer, 0, bytesSize);
                tempSize += additionalTempSize;

                if (contentDifference > 0)
                {
                    FileSize -= MainModel.MAX_BYTES_BUFFER_SIZE;
                    continue;
                }

                break;
            }

            message = "Отправлено данных - " + tempSize + ";";

            log(message, MainModel.LogType.Create, MainModel.ModelPart.Server);
            log("Файл отправлен;", MainModel.LogType.Create, MainModel.ModelPart.Server);

            model.server.fileStream.Close();

            log("Все потоки закрыты;", MainModel.LogType.Create, MainModel.ModelPart.Server);
        }

        private void sendFile(FileSystemEventArgs e)
        {
            try
            {
                sendFileToUser(e);
            }
            catch (Exception _e)
            {
                model.server.serverViewLogs.Items.Add(_e.Message);
            }
        }

        private void renameFile(string _oldName, string _name)
        {
            try
            {
                renameFileForUser(_oldName, _name);
            }
            catch (Exception _e)
            {
                model.server.serverViewLogs.Items.Add(_e.Message);
            }
        }

        private void renameFileForUser(string _name, string _newName)
        {
            string message = "Отправляем информацию о файле;";

            log(message, MainModel.LogType.Rename, MainModel.ModelPart.Server);

            model.server.networkStream = model.server.user.TcpClient.GetStream();
            byte[] byteSend = new byte[model.server.user.TcpClient.ReceiveBufferSize];
            
            FileInfo fInfoOld = new FileInfo(model.server.serverWatcher.Path + "\\" + _name);
            FileInfo fInfoNew = new FileInfo(model.server.serverWatcher.Path + "\\" + _newName);

            message = "Отправляем команду - " + model.server.Command + ";";

            log(message, MainModel.LogType.Rename, MainModel.ModelPart.Server);

            byte[] ByteCommand = new byte[MainModel.FIXED_BUFFER_SIZE];
            ByteCommand = System.Text.Encoding.UTF8.GetBytes(model.server.Command.ToCharArray());
            model.server.networkStream.Write(ByteCommand, 0, ByteCommand.Length);

            string FileNameOld = fInfoOld.Name;
            string FileNameNew = fInfoNew.Name;

            byte[] ByteFileName = new byte[MainModel.MAX_BYTES_BUFFER_SIZE];
            ByteFileName = System.Text.Encoding.UTF8.GetBytes(FileNameOld.ToCharArray());

            string FileLength;

            makeFormatedLenght(ref ByteFileName, out FileLength);

            message = "Отправляем длину старого имени - " + FileLength + ";";

            log(message, MainModel.LogType.Rename, MainModel.ModelPart.Server);

            byte[] ByteFileNameLength = new byte[MainModel.FIXED_BUFFER_SIZE];
            ByteFileNameLength = System.Text.Encoding.UTF8.GetBytes(FileLength.ToCharArray());
            model.server.networkStream.Write(ByteFileNameLength, 0, ByteFileNameLength.Length);


            message = ByteFileName.Length.ToString() + " - длина имени файла" + ";";

            log(message, MainModel.LogType.Rename, MainModel.ModelPart.Server);

            message = "Отправляем имя - " + FileNameOld + ";";

            log(message, MainModel.LogType.Rename, MainModel.ModelPart.Server);

            model.server.networkStream.Write(ByteFileName, 0, ByteFileName.Length);

            log("Старое имя отправлено;", MainModel.LogType.Rename, MainModel.ModelPart.Server);

            byte[] ByteFileNewName = new byte[MainModel.MAX_BYTES_BUFFER_SIZE];
            ByteFileNewName = System.Text.Encoding.UTF8.GetBytes(FileNameNew.ToCharArray());

            string NewFileLength = "";
            for (int count = 0; count < 2 - ByteFileNewName.Length.ToString().Length; count++)
                NewFileLength += "0";

            NewFileLength += "0" + ByteFileNewName.Length.ToString();

            message = "Отправляем длину новго имени - " + NewFileLength + ";";

            log(message, MainModel.LogType.Rename, MainModel.ModelPart.Server);

            byte[] ByteFileNewNameLength = new byte[MainModel.FIXED_BUFFER_SIZE];
            ByteFileNewNameLength = System.Text.Encoding.UTF8.GetBytes(NewFileLength.ToCharArray());
            model.server.networkStream.Write(ByteFileNewNameLength, 0, ByteFileNewNameLength.Length);

            message = ByteFileNewName.Length.ToString() + " - длина имени файла;";

            log(message, MainModel.LogType.Rename, MainModel.ModelPart.Server);

            message = "Отправляем имя - " + FileNameNew + ";";

            log(message, MainModel.LogType.Rename, MainModel.ModelPart.Server);

            model.server.networkStream.Write(ByteFileNewName, 0, ByteFileNewName.Length);

            log("Новое имя отправлено;", MainModel.LogType.Rename, MainModel.ModelPart.Server);

            model.server.fileStream.Close();

            log("Файл переименован у всех пользователям;", MainModel.LogType.Rename, MainModel.ModelPart.Server);
            log("Все потоки закрыты", MainModel.LogType.Rename, MainModel.ModelPart.Server);
        }

        private void deleteFileForUser(string _name)
        {
            byte[] ByteFileName = commonAction(_name, MainModel.LogType.Delete);

            log("Имя отправлено;", MainModel.LogType.Delete, MainModel.ModelPart.Server);

            log("Файл удалён у всех пользователям;", MainModel.LogType.Delete, MainModel.ModelPart.Server);

            model.server.fileStream.Close();

            log("Все потоки закрыты;", MainModel.LogType.Delete, MainModel.ModelPart.Server);
        }

        private void deleteFile(string _name)
        {
            try
            {
                deleteFileForUser(_name);
            }
            catch (Exception _e)
            {
                model.server.serverViewLogs.Items.Add(_e.Message);
            }
        }
        
        private void filterServerLogs(object sender, EventArgs e)
        {

            /*---------------------------------------------------------------*/

            model.server.serverViewLogs.Clear();

            /*---------------------------------------------------------------*/

            logsFiltration(MainModel.ModelPart.Server);

            /*---------------------------------------------------------------*/

        }

        private void clearServerLogs(object sender, EventArgs e)
        {
            ClearLogs(MainModel.ModelPart.Server);
        }

        #endregion

        #region Client implementation

        private void connectToServer(object sender, EventArgs e)
        {
            if (model.client.tcpClient != null && model.client.tcpClient.Connected)
            {
                MessageBox.Show("You are already connected!");
                return;
            }

            model.client.tcpClient = new TcpClient();

            try
            {
                model.client.tcpClient.Connect(
                    new IPEndPoint(
                            IPAddress.Parse(view.GetClientTextBoxIP())
                        , Convert.ToInt32(view.GetClientTextBoxPort())
                    )
                );
            }
            catch (Exception _e)
            {
                model.client.clientViewLogs.Items.Add(_e.Message);
            }

            if (model.client.tcpClient.Connected == true)
            {
                model.client.strRemote = model.client.tcpClient.GetStream();

                model.client.downloadThread = new Thread(StartReceivingCycle);
                model.client.downloadThread.IsBackground = true;
                model.client.downloadThread.Start();
            }
        }

        private void disconnectFromServer(object sender, EventArgs e)
        {
            if (model.client.tcpClient == null || !model.client.tcpClient.Connected || model.client.strRemote == null)
            {
                MessageBox.Show("You are not connected!");
                return;
            }

            string message = "Вы отключены от сервера!;";

            log(message, MainModel.LogType.Common, MainModel.ModelPart.Client);

            model.client.tcpClient.Close();
            model.client.downloadThread.Abort();
            model.client.strRemote.Close();
        }

        private void chooseClientFolder(object sender, EventArgs e)
        {
            DialogResult result = model.client.clientFolderDialog.ShowDialog();

            if (result == DialogResult.OK)
                model.client.path = model.client.clientFolderDialog.SelectedPath + "\\";
        }

        private void clientFilter(object sender, EventArgs e)
        {
            /*---------------------------------------------------------------*/

            model.client.clientViewLogs.Clear();

            /*---------------------------------------------------------------*/

            logsFiltration(MainModel.ModelPart.Client);

            /*---------------------------------------------------------------*/
        }

        private void clientClear(object sender, EventArgs e)
        {
            ClearLogs(MainModel.ModelPart.Client);
        }

        private void StartReceivingCycle()
        {
            while (true)
            {
                try
                {
                    ReceiveFile();
                }
                catch (Exception e)
                {
                    model.client.clientViewLogs.Items.Add(e.Message);
                }

                if (model.client.isFatalErrorOnServerSide)
                {
                    MessageBox.Show("Server unexpectedly shutdown!");
                    disconnectFromServer(this, null);
                    return;
                }

            }

        }

        private void ReceiveFile()
        {
            model.client.logsStorage[Convert.ToInt32(MainModel.LogType.Common)].AppendLine("Получение потока..." + ";");
            model.client.clientViewLogs.Items.Add("Получение потока...\r");

            byte[] byteCommand = new byte[MainModel.FIXED_BUFFER_SIZE];

            int bytesSize = 0;

            bytesSize = model.client.strRemote.Read(byteCommand, 0, MainModel.FIXED_BUFFER_SIZE);

            if (bytesSize == 0)
            {
                model.client.isFatalErrorOnServerSide = true;
                return;
            }

            string commandShortcut = System.Text.Encoding.UTF8.GetString(byteCommand, 0, bytesSize);

            model.client.logsStorage[Convert.ToInt32(MainModel.LogType.Common)].AppendLine("Считали команду - " + commandShortcut + ";");
            model.client.clientViewLogs.Items.Add("Считали команду - " + commandShortcut);

            byteCommand = new byte[MainModel.FIXED_BUFFER_SIZE];

            bytesSize = model.client.strRemote.Read(byteCommand, 0, MainModel.FIXED_BUFFER_SIZE);

            string fileNameBytesInterpretation = System.Text.Encoding.UTF8.GetString(byteCommand, 0, bytesSize);

            model.client.logsStorage[Convert.ToInt32(MainModel.LogType.Common)].AppendLine(
                "Считали битовое имя файла - " + fileNameBytesInterpretation + ";"
            );

            model.client.clientViewLogs.Items.Add("Считали битовое имя файла - " + fileNameBytesInterpretation);

            int fileNameLength = Convert.ToInt32(fileNameBytesInterpretation);

            byteCommand = new byte[fileNameLength];

            bytesSize = model.client.strRemote.Read(byteCommand, 0, fileNameLength);

            string serverFileName = System.Text.Encoding.UTF8.GetString(byteCommand, 0, bytesSize);

            model.client.logsStorage[Convert.ToInt32(MainModel.LogType.Common)].AppendLine(
                "Имя серверного файла файла - " + serverFileName + ";"
            );

            model.client.clientViewLogs.Items.Add("Имя серверного файла файла - " + serverFileName);

            byteCommand = new byte[MainModel.FIXED_BUFFER_SIZE];

            switch (commandShortcut)
            {
                case "crt":

                    createFileOperation(serverFileName, byteCommand, bytesSize);
                    break;

                case "rnm":

                    bytesSize = model.client.strRemote.Read(byteCommand, 0, MainModel.FIXED_BUFFER_SIZE);
                    fileNameBytesInterpretation = System.Text.Encoding.UTF8.GetString(byteCommand, 0, bytesSize);

                    model.client.logsStorage[Convert.ToInt32(MainModel.LogType.Rename)].Append(
                        "Считали длину имени файла - " + fileNameBytesInterpretation + ";"
                    );

                    model.client.clientViewLogs.Items.Add("Считали длину имени файла - " + fileNameBytesInterpretation);

                    fileNameLength = Convert.ToInt32(fileNameBytesInterpretation);

                    byteCommand = new byte[fileNameLength];

                    bytesSize = model.client.strRemote.Read(byteCommand, 0, fileNameLength);

                    string NewFileName = System.Text.Encoding.UTF8.GetString(byteCommand, 0, bytesSize);

                    model.client.logsStorage[Convert.ToInt32(MainModel.LogType.Rename)].Append(
                        "Обновалили файл. Новое имя файла - " + NewFileName + ";"
                    );

                    model.client.clientViewLogs.Items.Add("Обновалили файл. Новое имя файла - " + NewFileName);
                    File.Move(model.client.path + serverFileName, model.client.path + NewFileName);

                    break;

                case "del":

                    model.client.logsStorage[Convert.ToInt32(MainModel.LogType.Delete)].Append(
                        "Удалили файл под именем: " + serverFileName + ";"
                    );

                    model.client.clientViewLogs.Items.Add("Удалили файл под именем: " + serverFileName);
                    File.Delete(model.client.path + serverFileName);

                    break;
            }

        }

        private void createFileOperation(string _serverFileName, byte[] _byteCommands, int _bytesSize)
        {
            Encoding first = Encoding.Default;
            Encoding second = Encoding.Unicode;

            byte[] defaultBytes = first.GetBytes(_serverFileName);
            byte[] unicodeBytes = second.GetBytes(_serverFileName);

            FileStream strLocal = new FileStream(
                    model.client.path + _serverFileName
                , FileMode.Create
                , FileAccess.ReadWrite
                , FileShare.ReadWrite
            );

            model.client.logsStorage[Convert.ToInt32(MainModel.LogType.Create)].AppendLine(
                "Путь для хранения - " + model.client.path + _serverFileName + ";"
            );

            model.client.clientViewLogs.Items.Add("Путь для хранения - " + model.client.path + _serverFileName);

            _bytesSize = model.client.strRemote.Read(_byteCommands, 0, MainModel.FIXED_BUFFER_SIZE);

            string FileSizeLength = System.Text.Encoding.UTF8.GetString(_byteCommands, 0, _bytesSize);

            model.client.logsStorage[Convert.ToInt32(MainModel.LogType.Create)].AppendLine(
                "Считали длину имени файла - " + FileSizeLength + ";"
            );

            model.client.clientViewLogs.Items.Add("Считали длину имени файла - " + FileSizeLength);

            int bytesSizeInterpretation = Convert.ToInt32(FileSizeLength);

            _byteCommands = new byte[bytesSizeInterpretation];

            _bytesSize = model.client.strRemote.Read(_byteCommands, 0, bytesSizeInterpretation);

            long FileSize = Convert.ToInt64(System.Text.Encoding.UTF8.GetString(_byteCommands, 0, _bytesSize));

            model.client.logsStorage[Convert.ToInt32(MainModel.LogType.Create)].AppendLine(
                "Считали размер файла - " + FileSize + ";"
            );

            model.client.logsStorage[Convert.ToInt32(MainModel.LogType.Create)].AppendLine(
                "получен файл " + _serverFileName + " (" + FileSize + " байт);"
            );

            model.client.clientViewLogs.Items.Add("Считали размер файла - " + FileSize);
            model.client.clientViewLogs.Items.Add("получен файл " + _serverFileName + " (" + FileSize + " байт)\r");

            int tempSize = 0;

            _byteCommands = new byte[MainModel.MAX_BYTES_BUFFER_SIZE];

            if (FileSize > 0)
            {
                while (true)
                {
                    long bytesDifference = FileSize - MainModel.MAX_BYTES_BUFFER_SIZE;
                    int sizeToRead = _byteCommands.Length;
                    bool isBytesReaded = false;


                    if (bytesDifference > 0)
                    {
                        if (strLocal.Length == tempSize)
                        {
                            FileSize -= MainModel.MAX_BYTES_BUFFER_SIZE;
                            tempSize += MainModel.MAX_BYTES_BUFFER_SIZE;
                        }
                        else
                            sizeToRead = tempSize - Convert.ToInt32(strLocal.Length);

                        _bytesSize = model.client.strRemote.Read(_byteCommands, 0, sizeToRead);
                    }
                    else if (bytesDifference == 0)
                    {
                        tempSize += MainModel.MAX_BYTES_BUFFER_SIZE;
                        _bytesSize = model.client.strRemote.Read(_byteCommands, 0, sizeToRead);

                        isBytesReaded = true;
                    }
                    else
                    {
                        sizeToRead = Convert.ToInt32(FileSize);
                        _byteCommands = new byte[sizeToRead];
                        _bytesSize = model.client.strRemote.Read(_byteCommands, 0, sizeToRead);

                        tempSize += _bytesSize;
                        isBytesReaded = true;
                    }

                    strLocal.Write(_byteCommands, 0, _bytesSize);

                    if (isBytesReaded)
                        break;

                }
            }

            strLocal.Close();

            model.client.logsStorage[Convert.ToInt32(MainModel.LogType.Create)].AppendLine(
                "Считали содержимое файла;"
            );

            model.client.logsStorage[Convert.ToInt32(MainModel.LogType.Create)].AppendLine(
                "Считали данных - " + tempSize + ";"
            );

            model.client.logsStorage[Convert.ToInt32(MainModel.LogType.Create)].AppendLine(
               "Считали содержимое файла;"
            );

            model.client.clientViewLogs.Items.Add("Считали содержимое файла");
            model.client.clientViewLogs.Items.Add("");

            model.client.clientViewLogs.Items.Add("Считали данных - " + tempSize);
            model.client.clientViewLogs.Items.Add("Считали содержимое файла");
            model.client.clientViewLogs.Items.Add("");
        }


        #endregion
        
        #region Helper-functions

        /*-----------------------------------------------------------------*/

        private void logsFiltration(MainModel.ModelPart _part)
        {
            /*---------------------------------------------------------------*/

            int logsTypesCount = Convert.ToInt32(MainModel.LogType.Count);

            StringBuilder[] headers = new StringBuilder[logsTypesCount];
            for (int i = 0; i < logsTypesCount; i++)
                headers[i] = new StringBuilder();

            /*---------------------------------------------------------------*/

            headers[Convert.ToInt32(MainModel.LogType.Create)].AppendLine("------Create logsStorage------");
            headers[Convert.ToInt32(MainModel.LogType.Rename)].AppendLine("------Rename logsStorage------");
            headers[Convert.ToInt32(MainModel.LogType.Delete)].AppendLine("------Delete logsStorage------");
            headers[Convert.ToInt32(MainModel.LogType.Common)].AppendLine("------Common logsStorage------");

            StringBuilder[] storagedLogsGroup = null;
            ListView viewField = null;

            if ( _part == MainModel.ModelPart.Server )
            {
                storagedLogsGroup = model.server.logsStorage;
                viewField = model.server.serverViewLogs;
            }
            else
            {
                storagedLogsGroup = model.client.logsStorage;
                viewField = model.client.clientViewLogs;
            }

            for (int i = Convert.ToInt32(MainModel.LogType.Create); i < logsTypesCount; i++)
            {
                viewField.Items.Add(headers[i].ToString());

                var enumarableLogs = storagedLogsGroup[i].ToString().Split(';');

                foreach (var eachLog in enumarableLogs)
                    viewField.Items.Add(eachLog + "\n");

                viewField.Items.Add("\n");
            }

            /*---------------------------------------------------------------*/

        }


        /*-----------------------------------------------------------------*/

        private void log(
                string _message
            ,   MainModel.LogType _type
            ,   MainModel.ModelPart _part
        )
        {
            int type = Convert.ToInt32(_type);

            if ( _part == MainModel.ModelPart.Client )
            {
                model.client.clientViewLogs.Items.Add(_message);
                model.client.logsStorage[type].AppendLine(_message);
            }
            else
            {
                model.server.serverViewLogs.Items.Add(_message);
                model.server.logsStorage[type].AppendLine(_message);
            }
        }

        private void AllocateMemoryForLogs(MainModel.ModelPart _part)
        {
            var container = _part == MainModel.ModelPart.Server ?
                    model.client.logsStorage : model.server.logsStorage;

            for ( int i = 0; i < Convert.ToInt32(Model.LogType.Count); i++ )
                container[i] = new StringBuilder();
        }

        /*-----------------------------------------------------------------*/

        private void ClearLogs(MainModel.ModelPart _part)
        {
            StringBuilder[] commonContainer = null;
            ListView viewContainer = null;

            if ( _part == MainModel.ModelPart.Server )
            {
                commonContainer = model.server.logsStorage;
                viewContainer = model.server.serverViewLogs;
            }
            else
            {
                commonContainer = model.client.logsStorage;
                viewContainer = model.client.clientViewLogs;
            }

            foreach ( var eachBuilder in commonContainer )
                eachBuilder.Clear();

            viewContainer.Clear();

            viewContainer.Items.Add("Your logs have been cleared!;");
        }

        /*-----------------------------------------------------------------*/

        #endregion

    }

}
