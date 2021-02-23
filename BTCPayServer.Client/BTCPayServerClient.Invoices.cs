using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BTCPayServer.Client.Models;

namespace BTCPayServer.Client
{
    public partial class BTCPayServerClient
    {
        public virtual async Task<IEnumerable<InvoiceData>> GetInvoices(string storeId, bool includeArchived = false,
            CancellationToken token = default)
        {
            var response =
                await _httpClient.SendAsync(
                    CreateHttpRequest($"api/v1/stores/{storeId}/invoices",
                        new Dictionary<string, object>() {{nameof(includeArchived), includeArchived}}), token);
            return await HandleResponse<IEnumerable<InvoiceData>>(response);
        }

        public virtual async Task<InvoiceData> GetInvoice(string storeId, string invoiceId,
            CancellationToken token = default)
        {
            var response = await _httpClient.SendAsync(
                CreateHttpRequest($"api/v1/stores/{storeId}/invoices/{invoiceId}"), token);
            return await HandleResponse<InvoiceData>(response);
        }
        public virtual async Task<InvoicePaymentMethodDataModel[]> GetInvoicePaymentMethods(string storeId, string invoiceId,
            CancellationToken token = default)
        {
            var response = await _httpClient.SendAsync(
                CreateHttpRequest($"api/v1/stores/{storeId}/invoices/{invoiceId}/payment-methods"), token);
            return await HandleResponse<InvoicePaymentMethodDataModel[]>(response);
        }

        public virtual async Task ArchiveInvoice(string storeId, string invoiceId,
            CancellationToken token = default)
        {
            var response = await _httpClient.SendAsync(
                CreateHttpRequest($"api/v1/stores/{storeId}/invoices/{invoiceId}",
                    method: HttpMethod.Delete), token);
            await HandleResponse(response);
        }

        public virtual async Task<InvoiceData> CreateInvoice(string storeId,
            CreateInvoiceRequest request, CancellationToken token = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var response = await _httpClient.SendAsync(
                CreateHttpRequest($"api/v1/stores/{storeId}/invoices", bodyPayload: request,
                    method: HttpMethod.Post), token);
            return await HandleResponse<InvoiceData>(response);
        }

        public virtual async Task<InvoiceData> UpdateInvoice(string storeId, string invoiceId,
            UpdateInvoiceRequest request, CancellationToken token = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            var response = await _httpClient.SendAsync(
                CreateHttpRequest($"api/v1/stores/{storeId}/invoices/{invoiceId}", bodyPayload: request,
                    method: HttpMethod.Put), token);
            return await HandleResponse<InvoiceData>(response);
        }

        public virtual async Task<InvoiceData> MarkInvoiceStatus(string storeId, string invoiceId,
            MarkInvoiceStatusRequest request, CancellationToken token = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (!new[]{InvoiceStatus.New, InvoiceStatus.Settled, InvoiceStatus.Invalid}.Contains(request.Status))
                throw new ArgumentOutOfRangeException(nameof(request.Status), "Status can only be New, Invalid or Complete");
            var response = await _httpClient.SendAsync(
                CreateHttpRequest($"api/v1/stores/{storeId}/invoices/{invoiceId}/status", bodyPayload: request,
                    method: HttpMethod.Post), token);
            return await HandleResponse<InvoiceData>(response);
        }

        public virtual async Task<InvoiceData> UnarchiveInvoice(string storeId, string invoiceId, CancellationToken token = default)
        {
            var response = await _httpClient.SendAsync(
                CreateHttpRequest($"api/v1/stores/{storeId}/invoices/{invoiceId}/unarchive", 
                    method: HttpMethod.Post), token);
            return await HandleResponse<InvoiceData>(response);
        }
    }
}
