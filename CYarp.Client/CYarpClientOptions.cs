﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;

namespace CYarp.Client
{
    /// <summary>
    /// 客户端选项
    /// </summary>
    public class CYarpClientOptions
    {
        private static readonly string[] targetSchemes = [Uri.UriSchemeHttp, Uri.UriSchemeHttps];
        private static readonly string[] serverSchemes = [Uri.UriSchemeHttp, Uri.UriSchemeHttps, Uri.UriSchemeWs, Uri.UriSchemeWss];

        /// <summary>
        /// CYarp服务器Uri
        /// 支持http、https、 ws和wss
        /// </summary>
        [AllowNull]
        public Uri ServerUri { get; set; }

        /// <summary>
        /// 目标服务器Uri
        /// 支持http和https
        /// </summary>
        [AllowNull]
        public Uri TargetUri { get; set; }

        /// <summary>
        /// 目标服务器的UnixDomainSocket路径[可选]
        /// </summary>
        public string? TargetUnixDomainSocket { get; set; }

        /// <summary>
        /// 连接到CYarp服务器的Authorization请求头的值
        /// </summary>
        [AllowNull]
        public string Authorization { get; set; }

        /// <summary>
        /// 与server或target的连接超时时长
        /// 默认为5s
        /// </summary>
        public TimeSpan ConnectTimeout { get; set; } = TimeSpan.FromSeconds(5);

        /// <summary>
        /// 隧道传输错误回调
        /// </summary>
        public Action<Exception>? TunnelErrorCallback;

        /// <summary>
        /// 心跳包周期
        /// 默认30s
        /// </summary>
        public TimeSpan KeepAliveInterval { get; set; } = TimeSpan.FromSeconds(30d);

        /// <summary>
        /// 验证参数
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public void Validate()
        {
            ArgumentNullException.ThrowIfNull(TargetUri);
            ArgumentNullException.ThrowIfNull(ServerUri);
            ArgumentException.ThrowIfNullOrEmpty(Authorization);
            ArgumentOutOfRangeException.ThrowIfLessThan(ConnectTimeout, TimeSpan.Zero);

            if (targetSchemes.Contains(this.TargetUri.Scheme) == false)
            {
                throw new ArgumentException($"Scheme must be {string.Join(", ", targetSchemes)}", nameof(TargetUri));
            }

            if (serverSchemes.Contains(this.ServerUri.Scheme) == false)
            {
                throw new ArgumentException($"Scheme must be {string.Join(", ", serverSchemes)}", nameof(ServerUri));
            }

            if (AuthenticationHeaderValue.TryParse(this.Authorization, out _) == false)
            {
                throw new ArgumentException("Authorization format is incorrect", nameof(Authorization));
            }
        }
    }
}
