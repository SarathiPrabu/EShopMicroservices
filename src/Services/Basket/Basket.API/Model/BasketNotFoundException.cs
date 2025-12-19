namespace Basket.API.Model;

public class BasketNotFoundException(string userName) : NotFoundException("Basket", userName);