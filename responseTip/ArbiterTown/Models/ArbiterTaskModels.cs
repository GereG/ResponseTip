using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace ArbiterTown.Models
{
    public class ImageEvaluationTask
    {
        public int id { get; set; }
        public byte[] image1 { get; set; }
        public byte[] image2 { get; set; }
        public int chosenImage { get; set; }
    }

    public class TextAnswerValidationTask
    {
        public int id { get; set; }
        [Required()]
        [StringLength(128)]
        public string AspNetUsersId { get; set; }
        [Required()]
        public int ResponseTipTaskID { get; set; }
        [Required()]
        public DateTime timeAssigned { get; set; }
        [Required()]
        public TimeSpan timeBeforeExpired { get; set; }
        [Required()]
        public TextAnswerValidation_ArbiterAnswerEnum arbiterAnswer { get; set; }
        [Required()]
        public ArbiterTaskStatusesEnum taskStatus { get; set; }
    }

    public enum TextAnswerValidation_ArbiterAnswerEnum
    {
        notValid=0, Valid=1, skip=2
    }

    public enum ArbiterTaskStatusesEnum
    {
        created=0, expired=1, answered=2, finishedInAgreement=3,finishedInDisagreement=4
    }
}