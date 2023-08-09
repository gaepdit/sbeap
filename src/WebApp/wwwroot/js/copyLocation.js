// Copy site location address to mailing address
const copySiteAddressButton = document.getElementById("copy-site-location");

copySiteAddressButton.addEventListener("click", () => {
    document.getElementById("Item_MailingAddress_Street").value = document.getElementById("Item_Location_Street").value;
    document.getElementById("Item_MailingAddress_Street2").value = document.getElementById("Item_Location_Street2").value;
    document.getElementById("Item_MailingAddress_City").value = document.getElementById("Item_Location_City").value;
    document.getElementById("Item_MailingAddress_State").value = document.getElementById("Item_Location_State").value;
    document.getElementById("Item_MailingAddress_PostalCode").value = document.getElementById("Item_Location_PostalCode").value;
});
