using Microsoft.AspNetCore.Mvc;

namespace BookSearchAPI.FakeControllers
{
    [Route("FakeAWSSearch")]
    public class FakeAWSSearchController
    {

        private static string RESPONSE = "<Item> <ASIN>B00KOKTZLQ</ASIN> <OfferSummary> <LowestNewPrice> <Amount>3998</Amount> <CurrencyCode>USD</CurrencyCode> <FormattedPrice>$39.98</FormattedPrice> </LowestNewPrice>  <TotalNew>4</TotalNew> <TotalUsed>0</TotalUsed> <TotalCollectible>0</TotalCollectible> <TotalRefurbished>0</TotalRefurbished> </OfferSummary> <Offers> <TotalOffers>1</TotalOffers> <TotalOfferPages>1</TotalOfferPages> <MoreOffersUrl></MoreOffersUrl> <Offer> <OfferAttributes> <Condition>New</Condition> </OfferAttributes> <OfferListing> <OfferListingId>  </OfferListingId> <Price>  <Amount>6000</Amount>  <CurrencyCode>USD</CurrencyCode>  <FormattedPrice>$60.00</FormattedPrice> </Price> <SalePrice><Amount>4495</Amount><CurrencyCode>USD</CurrencyCode><FormattedPrice>$44.95</FormattedPrice></SalePrice><AmountSaved><Amount>1505</Amount><CurrencyCode>USD</CurrencyCode><FormattedPrice>$15.05</FormattedPrice></AmountSaved><PercentageSaved>25</PercentageSaved><Availability>Usually ships in 1-2 business days</Availability><AvailabilityAttributes><AvailabilityType>now</AvailabilityType><MinimumHours>24</MinimumHours><MaximumHours>48</MaximumHours></AvailabilityAttributes><IsEligibleForSuperSaverShipping>0</IsEligibleForSuperSaverShipping><IsEligibleForPrime>0</IsEligibleForPrime></OfferListing></Offer></Offers></Item>";

        [HttpGet]
        public string SearchForBook(string request)
        {
            return RESPONSE;
        }
    }
}