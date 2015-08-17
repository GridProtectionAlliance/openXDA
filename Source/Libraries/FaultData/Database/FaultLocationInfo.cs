using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace FaultData.Database
{
    partial class FaultEmailTemplate
    {
        public List<Recipient> GetRecipients(Table<Recipient> recipientTable)
        {
            List<int> recipientIDs = FaultEmailTemplateRecipients
                .Select(faultEmailTemplateRecipient => faultEmailTemplateRecipient.RecipientID)
                .ToList();

            return recipientTable
                .Where(recipient => recipientIDs.Contains(recipient.ID))
                .ToList();
        }
    }
}
