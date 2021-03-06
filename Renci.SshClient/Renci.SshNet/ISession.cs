﻿using System;
using System.Net.Sockets;
using System.Threading;
using Renci.SshNet.Channels;
using Renci.SshNet.Common;
using Renci.SshNet.Messages;
using Renci.SshNet.Messages.Authentication;
using Renci.SshNet.Messages.Connection;

namespace Renci.SshNet
{
    /// <summary>
    /// Provides functionality to connect and interact with SSH server.
    /// </summary>
    internal interface ISession
    {
        ///// <summary>
        ///// Gets or sets the connection info.
        ///// </summary>
        ///// <value>The connection info.</value>
        IConnectionInfo ConnectionInfo { get; }

        /// <summary>
        /// Gets a value indicating whether the session is connected.
        /// </summary>
        /// <value>
        /// <c>true</c> if the session is connected; otherwise, <c>false</c>.
        /// </value>
        bool IsConnected { get; }

        /// <summary>
        /// Gets the next channel number.
        /// </summary>
        /// <value>
        /// The next channel number.
        /// </value>
        uint NextChannelNumber { get; }

        /// <summary>
        /// Gets the session semaphore that controls session channels.
        /// </summary>
        /// <value>
        /// The session semaphore.
        /// </value>
        SemaphoreLight SessionSemaphore { get; }

        /// <summary>
        /// Create a new SSH session channel.
        /// </summary>
        /// <returns>
        /// A new SSH session channel.
        /// </returns>
        IChannelSession CreateChannelSession();

        /// <summary>
        /// Create a new channel for a locally forwarded TCP/IP port.
        /// </summary>
        /// <returns>
        /// A new channel for a locally forwarded TCP/IP port.
        /// </returns>
        IChannelDirectTcpip CreateChannelDirectTcpip();

        /// <summary>
        /// Creates a "forwarded-tcpip" SSH channel.
        /// </summary>
        /// <returns>
        /// A new "forwarded-tcpip" SSH channel.
        /// </returns>
        IChannelForwardedTcpip CreateChannelForwardedTcpip(uint remoteChannelNumber, uint remoteWindowSize, uint remoteChannelDataPacketSize);

       /// <summary>
        /// Registers SSH message with the session.
        /// </summary>
        /// <param name="messageName">The name of the message to register with the session.</param>
        void RegisterMessage(string messageName);

        /// <summary>
        /// Sends a message to the server.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <exception cref="SshConnectionException">The client is not connected.</exception>
        /// <exception cref="SshOperationTimeoutException">The operation timed out.</exception>
        /// <exception cref="InvalidOperationException">The size of the packet exceeds the maximum size defined by the protocol.</exception>
        void SendMessage(Message message);

        /// <summary>
        /// Unregister SSH message from the session.
        /// </summary>
        /// <param name="messageName">The name of the message to unregister with the session.</param>
        void UnRegisterMessage(string messageName);

        /// <summary>
        /// Waits for the specified handle or the exception handle for the receive thread
        /// to signal within the connection timeout.
        /// </summary>
        /// <param name="waitHandle">The wait handle.</param>
        /// <exception cref="SshConnectionException">A received package was invalid or failed the message integrity check.</exception>
        /// <exception cref="SshOperationTimeoutException">None of the handles are signaled in time and the session is not disconnecting.</exception>
        /// <exception cref="SocketException">A socket error was signaled while receiving messages from the server.</exception>
        /// <remarks>
        /// When neither handles are signaled in time and the session is not closing, then the
        /// session is disconnected.
        /// </remarks>
        void WaitOnHandle(WaitHandle waitHandle);

        /// <summary>
        /// Occurs when <see cref="ChannelCloseMessage"/> message received
        /// </summary>
        event EventHandler<MessageEventArgs<ChannelCloseMessage>> ChannelCloseReceived;

        /// <summary>
        /// Occurs when <see cref="ChannelDataMessage"/> message received
        /// </summary>
        event EventHandler<MessageEventArgs<ChannelDataMessage>> ChannelDataReceived;

        /// <summary>
        /// Occurs when <see cref="ChannelEofMessage"/> message received
        /// </summary>
        event EventHandler<MessageEventArgs<ChannelEofMessage>> ChannelEofReceived;

        /// <summary>
        /// Occurs when <see cref="ChannelExtendedDataMessage"/> message received
        /// </summary>
        event EventHandler<MessageEventArgs<ChannelExtendedDataMessage>> ChannelExtendedDataReceived;

        /// <summary>
        /// Occurs when <see cref="ChannelFailureMessage"/> message received
        /// </summary>
        event EventHandler<MessageEventArgs<ChannelFailureMessage>> ChannelFailureReceived;

        /// <summary>
        /// Occurs when <see cref="ChannelOpenConfirmationMessage"/> message received
        /// </summary>
        event EventHandler<MessageEventArgs<ChannelOpenConfirmationMessage>> ChannelOpenConfirmationReceived;

        /// <summary>
        /// Occurs when <see cref="ChannelOpenFailureMessage"/> message received
        /// </summary>
        event EventHandler<MessageEventArgs<ChannelOpenFailureMessage>> ChannelOpenFailureReceived;

        /// <summary>
        /// Occurs when <see cref="ChannelOpenMessage"/> message received
        /// </summary>
        event EventHandler<MessageEventArgs<ChannelOpenMessage>> ChannelOpenReceived;

        /// <summary>
        /// Occurs when <see cref="ChannelRequestMessage"/> message received
        /// </summary>
        event EventHandler<MessageEventArgs<ChannelRequestMessage>> ChannelRequestReceived;

        /// <summary>
        /// Occurs when <see cref="ChannelSuccessMessage"/> message received
        /// </summary>
        event EventHandler<MessageEventArgs<ChannelSuccessMessage>> ChannelSuccessReceived;

        /// <summary>
        /// Occurs when <see cref="ChannelWindowAdjustMessage"/> message received
        /// </summary>
        event EventHandler<MessageEventArgs<ChannelWindowAdjustMessage>> ChannelWindowAdjustReceived;

        /// <summary>
        /// Occurs when session has been disconnected from the server.
        /// </summary>
        event EventHandler<EventArgs> Disconnected;

        /// <summary>
        /// Occurs when an error occurred.
        /// </summary>
        event EventHandler<ExceptionEventArgs> ErrorOccured;

        /// <summary>
        /// Occurs when <see cref="RequestSuccessMessage"/> message received
        /// </summary>
        event EventHandler<MessageEventArgs<RequestSuccessMessage>> RequestSuccessReceived;

        /// <summary>
        /// Occurs when <see cref="RequestFailureMessage"/> message received
        /// </summary>
        event EventHandler<MessageEventArgs<RequestFailureMessage>> RequestFailureReceived;

        /// <summary>
        /// Occurs when <see cref="BannerMessage"/> message is received from the server.
        /// </summary>
        event EventHandler<MessageEventArgs<BannerMessage>> UserAuthenticationBannerReceived;
    }
}
