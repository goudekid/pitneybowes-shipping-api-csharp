/*
Copyright 2016 Pitney Bowes Inc.

Licensed under the MIT License(the "License"); you may not use this file except in compliance with the License.  
You may obtain a copy of the License in the README file or at
   https://opensource.org/licenses/MIT 
Unless required by applicable law or agreed to in writing, software distributed under the License is distributed 
on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the License 
for the specific language governing permissions and limitations under the License.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS 
OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

*/

using System.Collections.Generic;
using PitneyBowes.Developer.ShippingApi;

namespace PitneyBowes.Developer.ShippingApi.Fluent
{
    /// <summary>
    /// An address. If part of a response, this object also specifies address validation status, unless minimum validation is enabled.
    /// <a href="https://shipping.pitneybowes.com/reference/resource-objects.html#object-address"/>
    /// </summary>
    public class AddressFluent<T> where T : IAddress, new()
    {
        private T _address;

        /// <summary>
        /// Cast to the underlying type. Does not seem to work implicitly and needs an explicit cast - not sure why.
        /// </summary>
        /// <param name="a"></param>
        public static implicit operator T( AddressFluent<T> a)
        {
            return a._address;
        }

        /// <summary>
        /// Factory method to create an AddressFluent as the start of a method chain. 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static AddressFluent<T> Create(IAddress address = null)
        {
            var a = new AddressFluent<T>(address);
            return a;
        }

        /// <summary>
        /// Constructor - creates a new underlying object
        /// </summary>
        public AddressFluent(IAddress a)
        {
            if (a == null) a = new T();
            _address = (T)a;
        }

        /// <summary>
        /// Underlying Address object
        /// </summary>
        public T Address { get => _address; set { _address = value; } }

        /// <summary>
        /// Specify up to three address lines for the street address, including suite number.
        /// <a href="https://shipping.pitneybowes.com/reference/resource-objects.html#object-address"/>
        /// for considerations when specifying
        /// multiple lines in a shipment's ``fromAddress`` when
        /// ``MINIMAL_ADDRESS_VALIDATION`` is enabled.
        /// Street address and/or apartment and/or P.O. Box. You can specify up to
        /// three address lines.
        ///
        /// For USPS domestic destinations, ensure that the street address is
        /// specified as the last of the 3 address lines.This way, the street
        /// address is printed right above the city, state, postal zip code, per
        /// USPS label guidelines.
        /// </summary>                                                     
        public AddressFluent<T> AddressLines(string a1, string a2 = null, string a3 = null)
        {
            _address.AddAddressLine(a1);
            if (a2 != null) _address.AddAddressLine(a2);
            if (a3 != null) _address.AddAddressLine(a3);
            return this;
        }

        /// <summary>
        /// Sets city or town name
        /// </summary>
        /// <returns>this</returns>
        /// <param name="c">City or town name.</param>
        public AddressFluent<T> CityTown(string c) 
        {
            _address.CityTown = c;
            return this;
        }
        /// <summary>
        /// Sets the state or province.
        /// </summary>
        /// <returns>this</returns>
        /// <param name="s">For US address, use the 2-letter state code.</param>
        public AddressFluent<T> StateProvince(string s) 
        { 
            _address.StateProvince = s;
            return this;
        }
        /// <summary>
        /// Sets the postal code.
        /// </summary>
        /// <returns>this</returns>
        /// <param name="s"> For US addresses, either the 5-digit or 9-digit zip code.</param>
        public AddressFluent<T> PostalCode( string s) 
        {
            _address.PostalCode = s;
            return this;
        }
        /// <summary>
        /// Sets the country code
        /// </summary>
        /// <returns>this</returns>
        /// <param name="s">
        /// Two character country code. The country code is required for all shipments. <a href="https://shipping.pitneybowes.com/reference/resource-objects.html#object-address"/>
        /// </param>
        public AddressFluent<T> CountryCode(string s) 
        {
            _address.CountryCode = s;
            return this;
        }
        /// <summary>
        /// Sets the company name.
        /// </summary>
        /// <returns>this</returns>
        /// <param name="s">Company name</param>
        public AddressFluent<T> Company(string s) 
        {
            _address.Company = s;
            return this;
        }
        /// <summary>
        /// Name of sender or recipient.
        /// </summary>
        /// <returns>this</returns>
        /// <param name="s">Name</param>
        public AddressFluent<T> Name(string s) 
        {
            _address.Name=s;
            return this;
        }
        /// <summary>
        /// Phone number associated with the address
        /// </summary>
        /// <returns>this</returns>
        /// <param name="s">Phone number</param>
        public AddressFluent<T> Phone(string s) 
        { 
            _address.Phone = s;
            return this;
        }
        /// <summary>
        /// Email associated with the address
        /// </summary>
        /// <returns>this</returns>
        /// <param name="s">Email</param>
        public AddressFluent<T> Email(string s) 
        {
            _address.Email = s;
            return this;
        }
        /// <summary>
        /// Sets a value indicating whether this <see cref="T:PitneyBowes.Developer.ShippingApi.Model.Address"/>
        /// is residential. Indicates whether this is a residential address. It is recommended that
        /// this parameter be passed in as the address verification process is more
        /// accurate with it.
        /// </summary>
        /// <returns>this</returns>
        /// <param name="b">true means is residential<c>true</c> b.</param>
        public AddressFluent<T> Residential(bool b) 
        {
            _address.Residential = b;
            return this;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:PitneyBowes.Developer.ShippingApi.Model.Address"/> class. The response returns this field only if
        /// ``MINIMAL_ADDRESS_VALIDATION`` is **NOT** enabled.
        ///
        ///This indicates whether any action has been performed on the address during cleansing.
        /// </summary>
        /// <returns>This</returns>
        /// <param name="s">Status.</param>
        public  AddressFluent<T> Status(AddressStatus s) 
        { 
            _address.Status = s;
            return this;
        }
        /// <summary>
        /// Address validation verifies and cleanses postal addresses within the United
        /// States to help ensure packages are rated accurately and shipments arrive at
        /// their final destinations on time.This API call sends an address to be
        /// verified.The response returns a valid address.
        /// 1. Only U.S. domestic addresses are validated using this POST operation at this time.
        ///
        /// 2. Addresses are validated by the Pitney Bowes Address Validation (AV) engine.
        ///
        /// 3. Complete addresses, including address line(s) and city/state/zip are always
        /// validated by default. An error is returned if validation fails.
        ///
        /// - When the ``minimalAddressValidation`` query parameter flag is set to true,
        ///   the address line(s) are left as is and are not formatted, re-arranged 
        ///   included in the validation check.Only the city/state/zip line is
        ///   checked for validity
        ///   ** Note:** When using this option, please ensure that street addresses on
        ///   the labels are actually deliverable, as street addresses are not
        ///   validated by the Pitney Bowes AV engine.
        /// - When the ``minimalAddressValidation`` query parameter flag is set to false (by
        ///   default), the complete address, address line(s) and city/state/zip line
        ///   are included in the validation check.
        /// 4. Address validation returns 2-digit delivery points when available. You can find
        /// more information on delivery points at the |delivery-points|.
        /// .. |delivery-points| raw:: html
        /// <a href="https://en.wikipedia.org/wiki/Delivery_point" target="_blank">Wikipedia Delivery Point page</a>
        ///
        /// 5. If validation fails, you can use the :doc:`/api/post-address-suggest` API call
        /// to provide suggestions that could result in the address passing
        /// verification in a subsequent Address Validation API call.
        /// </summary>
        /// <returns>this</returns>
        public AddressFluent<T> Verify(ISession session = null)
        {
            if (session == null)
            {
                session = Globals.DefaultSession;
            }
            var addressResponse = Api.VerifyAddress(_address, session).GetAwaiter().GetResult();
            if (addressResponse.Success)
            {
                _address = addressResponse.APIResponse;
            }
            else
            {
                throw new ShippingAPIException(addressResponse);
            }
            return this;
        }
        /// <summary>
        /// Verifies the suggest address. This POST operation obtains suggested addresses in cases where the
        /// Address Validation API call has returned an error.
        /// 1. The suggested addresses are **not** validated addresses. You must reissue
        /// the API call.
        ///
        /// 2. Some suggestions might not be valid delivery points. This is especially
        /// true for range suggestions. For example, if the suggested valid range for a
        /// street is 1-100, the Suggest Addresses API call will consider all numbers 
        /// the range to be valid, even if the only valid delivery points are
        /// numbers 12, 24, and 36.
        ///
        /// 3. The operation provides several types of suggestions:
        /// - Range suggestions:
        /// - Primary number range (e.g., street number, PO Box number)
        /// - Secondary number range(e.g., suite number, apartment number
        /// - Street Name
        /// - City Name
        /// - Company Name
        /// - Puerto Rican Urbanization
        ///
        /// 4. The suggested addresses are **not** sorted by best match.
        ///
        /// 5. The API returns a maximum of 20 suggestions.
        ///
        /// 6. Some addresses might return no suggestions. If there are no
        /// suggestions, the ``suggestions`` object is not returned.
        /// </summary>
        /// <returns>this</returns>
        /// <param name="session">Session.</param>
        public IEnumerable<AddressFluent<T>> VerifySuggest(ISession session = null)
        {
            if (session == null)
            {
                session = Globals.DefaultSession;
            }

            var addressResponse = Api.VerifySuggestAddress<T>(_address, session).GetAwaiter().GetResult();
            if (addressResponse.Success)
            {
                _address = (T)addressResponse.APIResponse.Address;
                foreach( var a in addressResponse.APIResponse.Suggestions.Addresses)
                {
                    yield return new AddressFluent<T>((T)a);
                }
            }
            else
            {
                throw new ShippingAPIException(addressResponse);
            }

            yield break;
        }
        /// <summary>
        /// Person the specified name, phone and email.
        /// </summary>
        /// <returns>this</returns>
        /// <param name="name">Name.</param>
        /// <param name="phone">Phone.</param>
        /// <param name="email">Email.</param>
        public AddressFluent<T> Person( string name, string phone = null, string email = null)
        {
            return Name(name).Phone(phone).Email(email);
        }
    }
}