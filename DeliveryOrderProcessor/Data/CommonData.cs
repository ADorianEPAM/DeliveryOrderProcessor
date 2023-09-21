using System.Collections.Generic;

namespace DeliveryOrderProcessor.Data
{
    public readonly record struct DeliveryInfo(string id,
                                       Address shippingAddress,
                                       IEnumerable<WarehouseOrderInfo> orderInfo,
                                       string finalPrice);

    public readonly record struct Address(string Street,
                                          string City,
                                          string State,
                                          string Country,
                                          string ZipCode);

    public readonly record struct WarehouseOrderInfo(int Id, int Quantity);
}
