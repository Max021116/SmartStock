using System.Net.Http.Json;
using SmartStock.Shared;

namespace SmartStock.Client.Services;

public class SmartStockApiClient
{
    private readonly HttpClient _http;

    public SmartStockApiClient(HttpClient http) => _http = http;

    public async Task<PagedResult<ProductListDto>?> GetProductsAsync(int page = 1, string? search = null)
        => await _http.GetFromJsonAsync<PagedResult<ProductListDto>>(
            $"api/products?page={page}&pageSize=10&search={search}");

    public async Task<ProductDetailDto?> GetProductAsync(int id)
        => await _http.GetFromJsonAsync<ProductDetailDto>($"api/products/{id}");

    public async Task<IReadOnlyList<CategoryListDto>?> GetCategoriesAsync()
        => await _http.GetFromJsonAsync<IReadOnlyList<CategoryListDto>>("api/categories");

    public async Task<(bool Success, OrderPlacedDto? Order, ApiErrorResponse? Error, int StatusCode)>
        PlaceOrderAsync(PlaceOrderRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/orders", request);

        if (response.IsSuccessStatusCode)
        {
            var order = await response.Content.ReadFromJsonAsync<OrderPlacedDto>();
            return (true, order, null, (int)response.StatusCode);
        }

        var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        return (false, null, error, (int)response.StatusCode);
    }

    public async Task<(bool Success, ApiErrorResponse? Error, int StatusCode)>
        UpdateProductPriceAsync(int id, ProductUpdateDto dto)
    {
        var response = await _http.PutAsJsonAsync($"api/products/{id}", dto);
        var body = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();

        if (response.IsSuccessStatusCode)
            return (true, body, (int)response.StatusCode);

        return (false, body, (int)response.StatusCode);
    }
}