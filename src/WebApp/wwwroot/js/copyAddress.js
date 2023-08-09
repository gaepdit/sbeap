// Copy site mailing address to contact address
const copyMailingAddressButton = document.getElementById("copy-site-address");

copyMailingAddressButton.addEventListener("click", () => {
    document.getElementById("Item_Contact_Address_Street").value = document.getElementById("Item_MailingAddress_Street").value;
    document.getElementById("Item_Contact_Address_Street2").value = document.getElementById("Item_MailingAddress_Street2").value;
    document.getElementById("Item_Contact_Address_City").value = document.getElementById("Item_MailingAddress_City").value;
    document.getElementById("Item_Contact_Address_State").value = document.getElementById("Item_MailingAddress_State").value;
    document.getElementById("Item_Contact_Address_PostalCode").value = document.getElementById("Item_MailingAddress_PostalCode").value;
});
