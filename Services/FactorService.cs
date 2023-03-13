using ClosedXML.Excel;
using DenaAPI.DTO;
using DenaAPI.Interfaces;
using DenaAPI.Models;
using DenaAPI.Responses;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DenaAPI.Services
{
    public class FactorService : IFactorService
    {
        private readonly DenaDbContext denaDbContext;
        public FactorService(DenaDbContext denaDbContext)
        {
            this.denaDbContext = denaDbContext;
        }
        public async Task<FactorResponse> ExportExcelAsync()
        {
            var factors = await denaDbContext.Factors.ToListAsync();

            //using System.Data;  
            DataTable dt = new("Factor");
            dt.Columns.AddRange(new DataColumn[11] {
                new DataColumn("Id"),
                new DataColumn("ProductId"),
                new DataColumn("UserId"),
                new DataColumn("AttId"),
                new DataColumn("Number"),
                new DataColumn("Collect"),
                new DataColumn("CreatedDate"),
                new DataColumn("PostId"),
                new DataColumn("Payed"),
                new DataColumn("Accepted"),
                new DataColumn("Lapsed"),
            });

            foreach (var fact in factors) dt.Rows.Add(
                fact.Id,
                fact.ProductId,
                fact.UserId,
                fact.AttId,
                fact.Number,
                fact.Collect,
                fact.CreateDate,
                fact.PostId,
                fact.Payed,
                fact.Accepted,
                fact.Lapsed);

            return new FactorResponse
            {
                Success = true,
                ExportData = dt
            };




        }
        public async Task<FactorResponse> GetFacAsync(int FactorId)
        {
            if (denaDbContext.Factors == null)
            {
                return new FactorResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }
            var factor = await denaDbContext.Factors.FindAsync(FactorId);

            if (factor == null)
            {
                return new FactorResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }
            List<Factor> Factor = new() { factor };

            return new FactorResponse
            {
                Success = true,
                Factors = Factor
            };
        }
        public async Task<FactorResponse> DeleteFacAsync(int FactorId)
        {
            if (denaDbContext.Factors == null)
            {
                return new FactorResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }
            var factor = await denaDbContext.Factors.FindAsync(FactorId);
            if (factor == null)
            {
                return new FactorResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }

            denaDbContext.Factors.Remove(factor);
            await denaDbContext.SaveChangesAsync();

            return new FactorResponse
            {
                Success = true,
                Message = "Factor deleted!",
            };
        }
        public async Task<FactorResponse> GetFacsAsync()
        {
            if (denaDbContext.Factors == null)
            {
                return new FactorResponse
                {
                    Success = false,
                    Message = "Not found!",
                    ErrorCode = "404"
                };
            }
            return new FactorResponse
            {
                Success = true,
                Factors = await denaDbContext.Factors.ToListAsync()
            };
        }
        public async Task<FactorResponse> CreateFacAsync(FactorRequest factorRequest, PostDetailRequest postDetailRequest)
        {
            var postDetail = new PostDetail
            {
                Address = postDetailRequest.Address,
                Considreations = postDetailRequest.Considreations,
                Phone = postDetailRequest.Phone,
                Recipientname = postDetailRequest.Name
            };

            var factor = new Factor
            {
                UserId = factorRequest.UserId,
                CreateDate = DateTime.Now,
                AttId = factorRequest.AttId,
                ProductId = factorRequest.ProductId,
                Number = factorRequest.Number,
                Accepted = factorRequest.Accepted,
                Lapsed = factorRequest.Lapsed,
                Payed = factorRequest.Payed
            };

            await denaDbContext.Factors.AddAsync(factor);
            await denaDbContext.PostDetails.AddAsync(postDetail);

            var saveResponse = await denaDbContext.SaveChangesAsync();

            if (saveResponse >= 0)
            {
                return new FactorResponse { Success = true, Message = "Factor createed!" };
            }

            return new FactorResponse
            {
                Success = false,
                Error = "Unable to save the factor",
                ErrorCode = "500"
            };
        }
        public async Task<FactorResponse> UpdateFacAsync(int id, FactorRequest factorRequest)
        {
            var existingFactor = await denaDbContext.Factors.FindAsync(id);

            if (existingFactor == null)
            {
                return new FactorResponse
                {
                    Success = false,
                    Message = "Factor Not found!",
                    ErrorCode = "404"
                };
            }



            existingFactor.ProductId = factorRequest.ProductId;
            existingFactor.Number = factorRequest.Number;
            existingFactor.UserId = factorRequest.UserId;
            existingFactor.AttId = factorRequest.AttId;
            existingFactor.CreateDate = DateTime.Now;



            try
            {
                await denaDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FactorExists(existingFactor.Id))
                {
                    return new FactorResponse
                    {
                        Success = false,
                        Message = "Factor Not found!",
                        ErrorCode = "404"
                    };
                }
                else
                {
                    throw;
                }
            }

            return new FactorResponse
            {
                Success = true,
                Message = "Factor updated!",
            };
        }
        public async Task<FactorResponse> UpdatePostAsync(int id, PostDetailRequest postDetailRequest)
        {
            var existingPostDetails = await denaDbContext.PostDetails.FindAsync(id);

            if (existingPostDetails == null)
            {
                return new FactorResponse
                {
                    Success = false,
                    Message = "PostDetails Not found!",
                    ErrorCode = "404"
                };
            }



            existingPostDetails.Address = postDetailRequest.Address;
            existingPostDetails.Recipientname = postDetailRequest.Name;
            existingPostDetails.Phone = postDetailRequest.Phone;
            existingPostDetails.Considreations = postDetailRequest.Considreations;





            try
            {
                await denaDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FactorExists(existingPostDetails.Id))
                {
                    return new FactorResponse
                    {
                        Success = false,
                        Message = "Factor Not found!",
                        ErrorCode = "404"
                    };
                }
                else
                {
                    throw;
                }
            }

            return new FactorResponse
            {
                Success = true,
                Message = "Factor updated!",
            };
        }
        private bool FactorExists(int id)
        {
            return (denaDbContext.Factors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
